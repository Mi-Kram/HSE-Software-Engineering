using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OrderStatusChangeNotifier.Infrastructure.Services
{
    public abstract class MessageConsumer<T>(
        ILogger<T> logger,
        IConsumer<string, string> consumer) : BackgroundService
    {
        private readonly ILogger<T> logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IConsumer<string, string> consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    ConsumeResult<string, string> result = consumer.Consume(stoppingToken);
                    if (result.Message.Timestamp.UtcDateTime.AddMinutes(1) < DateTime.UtcNow) continue;
                    await HandleAsync(result.Message.Value, stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError("{Error}", ex.Message);
                }
            }
        }

        protected abstract Task HandleAsync(string message, CancellationToken stoppingToken);

        public override void Dispose()
        {
            consumer.Close();
            consumer.Dispose();
            base.Dispose();
        }
    }
}
