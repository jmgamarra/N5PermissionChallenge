using Microsoft.EntityFrameworkCore;
using Nest;

var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                    options.JsonSerializerOptions.MaxDepth = 64;
                });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
            //cfg.RegisterServicesFromAssembly(typeof(ModifyPermisoCommandHandler).Assembly);
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

