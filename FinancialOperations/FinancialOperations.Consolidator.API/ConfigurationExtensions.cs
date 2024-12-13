using FinancialOperations.Consolidator.API.Infrastructure.MQ.Consumers;
using MassTransit;

namespace FinancialOperations.Consolidator.API
{
    public static class ConfigurationExtensions
    {
        public static void AddMassTransitExtensions(this IServiceCollection service, IConfiguration configuration)
        {
            _ = service.AddMassTransit(x =>
            {
                x.AddConsumer<OperationConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    var host = configuration.GetSection("MQconfiguration:HostName").Value;
                    var queue = configuration.GetSection("MQconfiguration:QueueName").Value;

                    cfg.Host(host);
                    cfg.ReceiveEndpoint(queueName: queue, configureEndpoint: e =>
                    {
                        e.ConfigureConsumer<OperationConsumer>(context);
                    });
                });
            });

            service.AddHostedService<MassTransitHostedService>();
        }
    }
}
