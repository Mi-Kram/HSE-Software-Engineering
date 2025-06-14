using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PaymentsService.Domain.Entities;
using PaymentsService.Domain.Interfaces;

namespace PaymentsService.Infrastructure.Services
{
    public class PaidOrderProducer(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<PaidOrderProducer> logger,
        IProducer<string, string> producer,
        string topic,
        TimeSpan reserveTime) : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        private readonly ILogger<PaidOrderProducer> logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IProducer<string, string> producer = producer ?? throw new ArgumentNullException(nameof(producer));
        private readonly string topic = topic ?? throw new ArgumentNullException(nameof(topic));
        private readonly TimeSpan reserveTime = TimeSpan.Zero < reserveTime ? reserveTime : throw new ArgumentException($"{nameof(reserveTime)} должен быть больше 0");

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (await HandleAsync(stoppingToken)) continue;
                }
                catch (Exception ex)
                {
                    logger.LogError("{Message}", ex.Message);
                }

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }

        private async Task<bool> HandleAsync(CancellationToken stoppingToken)
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            IAccountService accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();

            PaidOrderMessage? msg = await accountService.TryReservePaidOrderMessageAsync(reserveTime);
            if (msg == null) return false;

            await producer.ProduceAsync(topic, new Message<string, string>()
            {
                Value = msg.Payload,
            }, stoppingToken);

            await accountService.DeletePaidOrderMessageAsync(msg.OrderID);
            return true;
        }

        public override void Dispose()
        {
            producer.Dispose();
            base.Dispose();
        }
    }
}
