using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        // Configura tus servicios aquí (por ejemplo, bases de datos, CQRS, etc.)
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      
            app.UseDeveloperExceptionPage();
        

        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
