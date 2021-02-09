using ExampleWebAPIApplication.Swagger;
using ExampleWebAPISApplication.Libraries;
using ExampleWebAPISApplication.Libraries.Interfaces;
using ExampleWebAPISApplication.Libraries.Telemetry;
using HealthChecks.UI.Client;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ExampleWebAPIApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ITelemetryInitializer>(_ => new MyTelemetryInitializer("ExampleWebAPI", Configuration["ApplicationInsightsKey"]) );
            services.AddApplicationInsightsTelemetry(Configuration["ApplicationInsightsKey"]);

            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy());

            services.AddControllers();

            // Configure API versioning
            services.AddApiVersioning(o => { o.ReportApiVersions = true; });
            services.AddVersionedApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'VVV";
                o.SubstituteApiVersionInUrl = true;
            });

            // Configure Swagger (OpenAPI doc generation)
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(o =>
            {
                o.OperationFilter<SwaggerDefaultValues>();
                o.IncludeXmlComments(System.IO.Path.Combine(System.AppContext.BaseDirectory, "ExampleWebAPIApplication.Comments.xml"));
            });

            services.AddSingleton<IMyCache>(_ => {
                MyCache.InitializeConnectionString(Configuration["CacheConnectionString"]);
                return new MyCache();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // Configure health check
            app.UseHealthChecks("/liveness", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self", System.StringComparison.InvariantCultureIgnoreCase)
            });
            app.UseHealthChecks("/hc", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Configure OpenAPI doc generation
            app.UseSwagger();
        }
    }
}
