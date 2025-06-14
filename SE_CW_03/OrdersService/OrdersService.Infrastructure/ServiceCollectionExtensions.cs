using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrdersService.Domain.Exceptions;
using OrdersService.Domain.Interfaces;
using OrdersService.Domain.Interfaces.MessagesRepository;
using OrdersService.Domain.Models;
using OrdersService.Infrastructure.Persistence;
using OrdersService.Infrastructure.Persistence.MessagesRepository;
using OrdersService.Infrastructure.Services;

namespace OrdersService.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            string? dbConnection = configuration.GetSection(ApplicationVariables.DB_CONNECTION).Value;
            if (string.IsNullOrWhiteSpace(dbConnection)) throw new EnvVariableException(ApplicationVariables.DB_CONNECTION);

            services.AddDbContext<OrdersDbContext>(opts => opts.UseNpgsql(dbConnection));
            services.AddScoped<IOrdersRepository, OrdersRepository>();
            services.AddScoped<INewOrderMessagesRepository, NewOrderMessagesRepository>();
            services.AddScoped<IPaidOrderMessagesRepository, PaidOrderMessagesRepository>();
            services.AddScoped<IDbTransaction, SimpleDbTransaction>();

            AddNewOrderProducer(services, configuration);
            AddOnPaidOrderResultConsumer(services, configuration);
        }

        private static void AddNewOrderProducer(IServiceCollection services, IConfiguration configuration)
        {
            string? kafka = configuration.GetSection(ApplicationVariables.KAFKA).Value;
            if (string.IsNullOrWhiteSpace(kafka)) throw new EnvVariableException(ApplicationVariables.KAFKA);

            string? topic = configuration.GetSection(ApplicationVariables.TOPIC_NEW_ORDER).Value;
            if (string.IsNullOrWhiteSpace(topic)) throw new EnvVariableException(ApplicationVariables.TOPIC_NEW_ORDER);

            services.AddHostedService(x =>
            {
                IServiceScopeFactory scopeFactory = x.GetRequiredService<IServiceScopeFactory>();
                ILogger<NewOrderProducer> logger = x.GetRequiredService<ILogger<NewOrderProducer>>();

                ProducerConfig config = new()
                {
                    BootstrapServers = kafka,
                    Acks = Acks.All,
                    EnableIdempotence = true,
                    AllowAutoCreateTopics = false
                };
                IProducer<string, string> producer = new ProducerBuilder<string, string>(config).Build();
                
                return new NewOrderProducer(scopeFactory, logger, producer, topic, TimeSpan.FromSeconds(10));
            });
        }

        private static void AddOnPaidOrderResultConsumer(IServiceCollection services, IConfiguration configuration)
        {
            string? kafka = configuration.GetSection(ApplicationVariables.KAFKA).Value;
            if (string.IsNullOrWhiteSpace(kafka)) throw new EnvVariableException(ApplicationVariables.KAFKA);

            string? topic = configuration.GetSection(ApplicationVariables.TOPIC_PAID_ORDER).Value;
            if (string.IsNullOrWhiteSpace(topic)) throw new EnvVariableException(ApplicationVariables.TOPIC_PAID_ORDER);

            services.AddHostedService(x =>
            {
                IServiceScopeFactory scopeFactory = x.GetRequiredService<IServiceScopeFactory>();
                ILogger<OnPaidOrderResultConsumer> logger = x.GetRequiredService<ILogger<OnPaidOrderResultConsumer>>();

                ConsumerConfig config = new()
                {
                    BootstrapServers = kafka,
                    GroupId = nameof(OrdersService),
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnableAutoCommit = false,
                    AllowAutoCreateTopics = false,
                };
                
                IConsumer<string, string> consumer = new ConsumerBuilder<string, string>(config).Build();
                consumer.Subscribe(topic);
                return new OnPaidOrderResultConsumer(scopeFactory, logger, consumer);
            });
        }
    }
}
