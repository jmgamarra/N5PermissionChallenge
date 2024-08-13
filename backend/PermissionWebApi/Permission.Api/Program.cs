var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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

// Configurar Kafka
var kafkaSettings = builder.Configuration.GetSection("Kafka");
var bootstrapServers = kafkaSettings["BootstrapServers"];
var topic = kafkaSettings["Topic"];

builder.Services.AddSingleton(new KafkaProducerService(bootstrapServers, topic));
builder.Services.AddSingleton(new KafkaConsumerService(bootstrapServers, topic));

var app = builder.Build();
// Usar CORS
app.UseCors("AllowAll");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseSwaggerUI(c =>
    //{
    //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    //    c.RoutePrefix = string.Empty; // Para hacer que Swagger esté disponible en la ruta raíz
    //});
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Ejecutar el consumidor en un background task
var consumerService = app.Services.GetRequiredService<KafkaConsumerService>();
var consumerTask = Task.Run(() => consumerService.Consume());

app.Run();
