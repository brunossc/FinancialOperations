using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace FinancialOperations.SideCar
{
    public static class AppLogger
    {
        public static void AddSerilogWithElastic(this IServiceCollection service, string serviceName, string environment)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://elasticsearch:9200"))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = "logs-{0:yyyy.MM.dd}",
                    MinimumLogEventLevel = Serilog.Events.LogEventLevel.Information
                })
                .CreateLogger();
        }
    }
}
