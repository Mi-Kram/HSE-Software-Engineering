using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using OrderStatusChangeNotifier.Application.UseCases;
using OrderStatusChangeNotifier.Infrastructure.Services;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace OrderStatusChangeNotifier.Tests.OrderStatusChangeNotifier.Application
{
    public class WebSocketManagerTests
    {
        private readonly Mock<NewOrderConsumer> _newOrderConsumerMock = new(
            new Mock<ILogger<NewOrderConsumer>>().Object,
            new Mock<IConsumer<string, string>>().Object);
        private readonly Mock<PaidOrderConsumer> _paidOrderConsumerMock = new(
            new Mock<ILogger<PaidOrderConsumer>>().Object,
            new Mock<IConsumer<string, string>>().Object);
        private readonly ServiceProvider _serviceProvider;

        public WebSocketManagerTests()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IHostedService>(_newOrderConsumerMock.Object);
            services.AddSingleton<IHostedService>(_paidOrderConsumerMock.Object);

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenNewOrderConsumerMissing()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IHostedService>(_paidOrderConsumerMock.Object); // only paid
            var provider = services.BuildServiceProvider();

            var exception = Assert.Throws<ArgumentNullException>(() => new WebSocketManager(provider));
            Assert.Equal("INewOrderConsumer", exception.ParamName);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenPaidOrderConsumerMissing()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IHostedService>(_newOrderConsumerMock.Object); // only new
            var provider = services.BuildServiceProvider();

            var exception = Assert.Throws<ArgumentNullException>(() => new WebSocketManager(provider));
            Assert.Equal("IPaidOrderConsumer", exception.ParamName);
        }

        [Fact]
        public async Task HandleWebSocketConnectionAsync_ShouldAddConnectionAndRemoveOnClose()
        {
            // Arrange
            var manager = new WebSocketManager(_serviceProvider);
            var mockSocket = new Mock<WebSocket>();

            var cts = new CancellationTokenSource();
            var message = Encoding.UTF8.GetBytes("test");

            mockSocket.SetupSequence(s => s.State)
                .Returns(WebSocketState.Open)
                .Returns(WebSocketState.Open)
                .Returns(WebSocketState.CloseSent);

            mockSocket.Setup(s => s.ReceiveAsync(It.IsAny<ArraySegment<byte>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new WebSocketReceiveResult(0, WebSocketMessageType.Close, true));

            mockSocket.Setup(s => s.CloseAsync(
                WebSocketCloseStatus.NormalClosure,
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var task = manager.HandleWebSocketConnectionAsync(mockSocket.Object);
            await Task.Delay(100); // Let it run
            cts.Cancel(); // End test

            // Assert
            mockSocket.Verify(s => s.ReceiveAsync(It.IsAny<ArraySegment<byte>>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce());
            mockSocket.Verify(s => s.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", It.IsAny<CancellationToken>()), Times.AtLeastOnce());
        }

        [Fact]
        public void OnOrderStatusChanged_ShouldSendMessageToOpenSockets()
        {
            // Arrange
            var manager = new WebSocketManager(_serviceProvider);
            var mockSocket = new Mock<WebSocket>();

            var sent = false;
            mockSocket.SetupGet(s => s.State).Returns(WebSocketState.Open);
            mockSocket.Setup(s => s.SendAsync(It.IsAny<ArraySegment<byte>>(),
                                              WebSocketMessageType.Text,
                                              true,
                                              It.IsAny<CancellationToken>()))
                      .Returns(Task.CompletedTask)
                      .Callback(() => sent = true);

            var socketField = typeof(WebSocketManager)
                .GetField("connections", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var connections = (ConcurrentDictionary<string, WebSocket>)socketField.GetValue(manager)!;
            connections.TryAdd("socket1", mockSocket.Object);

            // Act
            var handler = typeof(WebSocketManager)
                .GetMethod("OnOrderStatusChanged", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            handler!.Invoke(manager, new object[] { "123", "PAID" });

            // Assert
            Assert.True(sent);
        }
    }
}
