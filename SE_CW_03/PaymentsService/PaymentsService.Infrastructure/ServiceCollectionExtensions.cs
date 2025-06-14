using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PaymentsService.Domain.Exceptions;
using PaymentsService.Domain.Interfaces;
using PaymentsService.Domain.Interfaces.MessagesRepository;
using PaymentsService.Domain.Models;
using PaymentsService.Infrastructure.Persistence;
using PaymentsService.Infrastructure.Persistence.MessagesRepository;
using PaymentsService.Infrastructure.Services;

namespace PaymentsService.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            string? dbConnection = configuration.GetSection(ApplicationVariables.DB_CONNECTION).Value;
            if (string.IsNullOrWhiteSpace(dbConnection)) throw new EnvVariableException(ApplicationVariables.DB_CONNECTION);

            services.AddDbContext<AppDbContext>(opts => opts.UseNpgsql(dbConnection));
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IOrderToPayMessagesRepository, OrderToPayMessagesRepository>();
            services.AddScoped<IPaidOrderMessagesRepository, PaidOrderMessagesRepository>();
            services.AddScoped<IDbTransaction, SimpleDbTransaction>();

            AddOrderToPayConsumer(services, configuration);
            AddPaidOrderProducer(services, configuration);
        }

        private static void AddOrderToPayConsumer(IServiceCollection services, IConfiguration configuration)
        {
            string? kafka = configuration.GetSection(ApplicationVariables.KAFKA).Value;
            if (string.IsNullOrWhiteSpace(kafka)) throw new EnvVariableException(ApplicationVariables.KAFKA);

            string? topic = configuration.GetSection(ApplicationVariables.TOPIC_NEW_ORDER).Value;
            if (string.IsNullOrWhiteSpace(topic)) throw new EnvVariableException(ApplicationVariables.TOPIC_NEW_ORDER);

            services.AddHostedService(x =>
            {
                IServiceScopeFactory scopeFactory = x.GetRequiredService<IServiceScopeFactory>();
                ILogger<OnOrderToPayConsumer> logger = x.GetRequiredService<ILogger<OnOrderToPayConsumer>>();

                ConsumerConfig config = new()
                {
                    BootstrapServers = kafka,
                    GroupId = nameof(PaymentsService),
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnableAutoCommit = false,
                    AllowAutoCreateTopics = false,
                };

                IConsumer<string, string> consumer = new ConsumerBuilder<string, string>(config).Build();
                consumer.Subscribe(topic);
                return new OnOrderToPayConsumer(scopeFactory, logger, consumer);
            });
        }

        private static void AddPaidOrderProducer(IServiceCollection services, IConfiguration configuration)
        {
            string? kafka = configuration.GetSection(ApplicationVariables.KAFKA).Value;
            if (string.IsNullOrWhiteSpace(kafka)) throw new EnvVariableException(ApplicationVariables.KAFKA);

            string? topic = configuration.GetSection(ApplicationVariables.TOPIC_PAID_ORDER).Value;
            if (string.IsNullOrWhiteSpace(topic)) throw new EnvVariableException(ApplicationVariables.TOPIC_PAID_ORDER);

            services.AddHostedService(x =>
            {
                IServiceScopeFactory scopeFactory = x.GetRequiredService<IServiceScopeFactory>();
                ILogger<PaidOrderProducer> logger = x.GetRequiredService<ILogger<PaidOrderProducer>>();

                ProducerConfig config = new()
                {
                    BootstrapServers = kafka,
                    Acks = Acks.All,
                    EnableIdempotence = true,
                    AllowAutoCreateTopics = false
                };
                IProducer<string, string> producer = new ProducerBuilder<string, string>(config).Build();

                return new PaidOrderProducer(scopeFactory, logger, producer, topic, TimeSpan.FromSeconds(10));
            });
        }
    }
}
