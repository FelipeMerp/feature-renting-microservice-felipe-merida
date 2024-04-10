using GtMotive.Estimate.Microservice.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GtMotive.Estimate.Microservice.Host
{
    // Startup class that configures the ASP.NET Core application.
    public class Startup
    {
        // Constructor that receives the application configuration.
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // Property to store the application configuration.
        public IConfiguration Configuration { get; }

        // Method to configure the application services.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add application controllers
            services.AddControllers();

            // Register RentingDbContext as a service
            services.AddScoped<RentingDbContext>();
        }

        // Method to configure the application and its middleware.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable the development error page if the application is in development mode.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // More robust exception handling for production environments
                app.UseExceptionHandler("/Error");
                app.UseHsts(); // Redirect all HTTP requests to HTTPS in production environment
            }

            // Environment-specific configuration: redirect HTTP requests to HTTPS
            // app.UseHttpsRedirection();.
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
