using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using System;
using System.IO;

namespace ExampleWebAPIApplication
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateBootstrapLogger();
            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly. Message: {exMessage}", ex.Message);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    //.WriteTo.File(new JsonFormatter(renderMessage:  true), "log.json")
                    //.WriteTo.ApplicationInsights(TelemetryConfiguration.CreateDefault().InstrumentationKey = context.Configuration["ApplicationInsightsKey"], TelemetryConverter.Events)
                    //.WriteTo.DatadogLogs(context.Configuration["DatadogAPIKey"])
                )
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "mnt/secrets-store");
                    config
                        .AddKeyPerFile("/mnt/secrets-store", optional: true)
                        .AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
