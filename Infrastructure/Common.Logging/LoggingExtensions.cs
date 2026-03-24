using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Common.Logging;

public static class LoggingExtensions
{
    public static WebApplicationBuilder ConfigureCommonLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .WriteTo.Console();

            var elasticUri = context.Configuration["ElasticConfiguration:Uri"];
            if (string.IsNullOrWhiteSpace(elasticUri))
            {
                return;
            }

            var indexFormat = context.Configuration["ElasticConfiguration:IndexFormat"]
                              ?? $"{context.HostingEnvironment.ApplicationName?.ToLowerInvariant().Replace('.', '-')}-logs-{DateTime.UtcNow:yyyy.MM}";

            loggerConfiguration.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
            {
                AutoRegisterTemplate = true,
                IndexFormat = indexFormat
            });
        });

        return builder;
    }
}
