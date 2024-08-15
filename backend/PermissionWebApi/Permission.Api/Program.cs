using Microsoft.EntityFrameworkCore;
using Nest;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            options.JsonSerializerOptions.MaxDepth = 64;
        });

// Configuración de Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Configurar DBContext
var connectionString = builder.Configuration.GetConnectionString("N5Connection");
builder.Services.AddDbContext<PermissionsDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configurar MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(RequestPermisoCommandHandler).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GetPermisoByIdQueryHandler).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GetPermisosQueryHandler).Assembly);
});

// Configuración de Elasticsearch
var elasticsearchSettings = builder.Configuration.GetSection("Elasticsearch");
var elasticUrl = elasticsearchSettings["Url"];
var indexName = elasticsearchSettings["Index"];
builder.Services.AddSingleton<IElasticClient>(provider =>
{
    var settings = new ConnectionSettings(new Uri(elasticUrl))
                   .DefaultIndex(indexName);
    return new ElasticClient(settings);
});

// Configuración de Services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPermisoRepository, PermisoRepository>();

// Configurar Kafka
var kafkaSettings = builder.Configuration.GetSection("Kafka");
var bootstrapServers = kafkaSettings["BootstrapServers"];
var topic = kafkaSettings["Topic"];

builder.Services.AddSingleton(new KafkaProducerService(bootstrapServers, topic));
builder.Services.AddSingleton(new KafkaConsumerService(bootstrapServers, topic));

var app = builder.Build();

// Usar CORS
app.UseCors("AllowAll");

// Aplicar migraciones de base de datos
ApplyDatabaseMigrations(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Ejecutar el consumidor en un background task
var consumerService = app.Services.GetRequiredService<KafkaConsumerService>();
var consumerTask = Task.Run(() => consumerService.Consume());

app.Run();

void ApplyDatabaseMigrations(IHost app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<PermissionsDbContext>();
            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating the database.");
        }
    }
}
// Método añadido para las pruebas de integración
public partial class Program
{
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Program>(); // Referencia a Program como Startup
            });
}
