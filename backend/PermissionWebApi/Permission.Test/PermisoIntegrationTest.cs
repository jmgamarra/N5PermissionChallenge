using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Permission.Domain;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;

public class PermisoIntegrationTest
{
    private readonly HttpClient _client;

    public PermisoIntegrationTest()
    {
        // Configurar el servidor de pruebas
        var builder = new WebHostBuilder()
            .ConfigureServices(services =>
            {
                // Agregar servicios para pruebas
                services.AddControllers()
                        .AddJsonOptions(options =>
                        {
                            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                            options.JsonSerializerOptions.MaxDepth = 64;
                        });

                services.AddEndpointsApiExplorer();
                services.AddSwaggerGen();

                // Configura tus servicios aquí (similar a `Program.cs`)
                // Ejemplo:
                var connectionString = "YourConnectionString";
                services.AddDbContext<PermissionsDbContext>(options =>
                    options.UseSqlServer(connectionString));

                services.AddScoped<IUnitOfWork, UnitOfWork>();
                services.AddScoped<IPermisoRepository, PermisoRepository>();
                // Agrega otras configuraciones de servicios necesarias
            })
            .Configure(app =>
            {
                // Configurar el pipeline de la aplicación
                app.UseRouting();
                app.UseAuthorization();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    // Configura tus endpoints aquí (si es necesario)
                });
            });

        // Crear el servidor de pruebas y cliente
        var server = new TestServer(builder);
        _client = server.CreateClient();
    }

    [Fact]
    public async Task GetPermisos_ReturnsOkResponse1()
    {
        var response = await _client.GetAsync("/api/Permisos");
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseString);
    }

    [Fact]
    public async Task RequestPermiso_ReturnsCreatedResponse()
    {
        // Arrange
        var command = new RequestPermisoCommand { /* Initialize properties */ };
        var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/permiso", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Permiso>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.NotNull(result);
        // Further assertions based on your requirements
    }

    [Fact]
    public async Task ModifyPermiso_ReturnsOkResponse()
    {
        // Arrange
        var command = new ModifyPermisoCommand { Id = 1, /* Initialize properties */ };
        var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync("/api/permiso/1", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Permiso>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.NotNull(result);
        // Further assertions based on your requirements
    }

    [Fact]
    public async Task GetPermisos_ReturnsOkResponse()
    {
        // Act
        var response = await _client.GetAsync("/api/permiso");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<Permiso>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.NotNull(result);
        // Further assertions based on your requirements
    }
}
