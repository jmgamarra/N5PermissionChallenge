using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        return base.CreateHost(builder
            .ConfigureServices(services =>
            {
                // Aquí puedes agregar configuraciones adicionales si es necesario.
            }));
    }
}
