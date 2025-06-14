using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrderStatusChangeNotifier.Domain.Exceptions;
using OrderStatusChangeNotifier.Domain.Models;
using OrderStatusChangeNotifier.Infrastructure.Services;

namespace OrderStatusChangeNotifier.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddPaidOrderConsumer(services, configuration);
            AddNewOrderConsumer(services, configuration);
        }

        private static void AddNewOrderConsumer(IServiceCollection services, IConfiguration configuration)
        {
            string? kafka = configuration.GetSection(ApplicationVariables.KAFKA).Value;
            if (string.IsNullOrWhiteSpace(kafka)) throw new EnvVariableException(ApplicationVariables.KAFKA);

            string? topic = configuration.GetSection(ApplicationVariables.TOPIC_NEW_ORDER).Value;
            if (string.IsNullOrWhiteSpace(topic)) throw new EnvVariableException(ApplicationVariables.TOPIC_NEW_ORDER);

            services.AddHostedService(x =>
            {
                ILogger<NewOrderConsumer> logger = x.GetRequiredService<ILogger<NewOrderConsumer>>();

                ConsumerConfig config = new()
                {
                    BootstrapServers = kafka,
                    GroupId = nameof(OrderStatusChangeNotifier),
                    AutoOffsetReset = AutoOffsetReset.Latest,
                    EnableAutoCommit = true,
                    AllowAutoCreateTopics = false,
                };

                IConsumer<string, string> consumer = new ConsumerBuilder<string, string>(config).Build();
                consumer.Subscribe(topic);
                return new NewOrderConsumer(logger, consumer);
            });
        }

        private static void AddPaidOrderConsumer(IServiceCollection services, IConfiguration configuration)
        {
            string? kafka = configuration.GetSection(ApplicationVariables.KAFKA).Value;
            if (string.IsNullOrWhiteSpace(kafka)) throw new EnvVariableException(ApplicationVariables.KAFKA);

            string? topic = configuration.GetSection(ApplicationVariables.TOPIC_PAID_ORDER).Value;
            if (string.IsNullOrWhiteSpace(topic)) throw new EnvVariableException(ApplicationVariables.TOPIC_PAID_ORDER);

            services.AddHostedService(x =>
            {
                ILogger<PaidOrderConsumer> logger = x.GetRequiredService<ILogger<PaidOrderConsumer>>();

                ConsumerConfig config = new()
                {
                    BootstrapServers = kafka,
                    GroupId = nameof(OrderStatusChangeNotifier),
                    AutoOffsetReset = AutoOffsetReset.Latest,
                    EnableAutoCommit = true,
                    AllowAutoCreateTopics = false,
                };

                IConsumer<string, string> consumer = new ConsumerBuilder<string, string>(config).Build();
                consumer.Subscribe(topic);
                return new PaidOrderConsumer(logger, consumer);
            });
        }
    }
}
