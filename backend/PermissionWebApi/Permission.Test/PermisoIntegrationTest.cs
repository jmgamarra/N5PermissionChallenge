using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Permission.Domain;
using System.Text;
using System.Text.Json;

public class PermisoIntegrationTest
{
    private readonly HttpClient _client;

    public PermisoIntegrationTest()
    {
        var hostBuilder = new HostBuilder()
            .ConfigureWebHost(webHost =>
            {
                webHost.UseTestServer()
                       .UseStartup<Program>(); // Asegúrate de que la clase `Program` esté accesible.

                // Si `Program` no es accesible, puedes usar una clase `Startup` personalizada o similar.
            });

        var host = hostBuilder.StartAsync().GetAwaiter().GetResult();
        _client = host.GetTestClient();
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
