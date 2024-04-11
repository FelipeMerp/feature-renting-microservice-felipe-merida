using GtMotive.Estimate.Microservice.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GtMotive.Estimate.Microservice.Host
{
    /// <summary>
    /// Startup class that configures the ASP.NET Core application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// Constructor that receives the application configuration.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets property to store the application configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Method to configure the application services.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add application controllers
            services.AddControllers();

            // Register RentingDbContext as a service
            services.AddSingleton<RentingDbContext>();
        }

        /// <summary>
        /// Method to configure the application and its middleware.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The web host environment.</param>
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
