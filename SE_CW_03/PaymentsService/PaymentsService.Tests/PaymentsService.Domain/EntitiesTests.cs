using PaymentsService.Domain.Entities;

namespace PaymentsService.Tests.PaymentsService.Domain
{
    public class EntitiesTests
    {
        [Fact]
        public void Account_Test()
        {
            Account account = new()
            {
                UserID = 5,
                Balance = 25,
                Caption = "Test"
            };

            Assert.Equal(5, account.UserID);
            Assert.Equal(25, account.Balance);
            Assert.Equal("Test", account.Caption);

            Assert.Throws<ArgumentNullException>(() => account.Caption = null!);
        }

        [Fact]
        public void OrderToPayMessage_Test()
        {
            DateTime dt1 = DateTime.Now, dt2 = DateTime.Now.AddMinutes(2);

            OrderToPayMessage message = new()
            {
                OrderID = Guid.NewGuid(),
                UserID = 5,
                Bill = 25,
                CreatedAt = dt1,
                Reserved = dt2
            };

            Assert.NotEmpty(message.OrderID.ToString());
            Assert.Equal(5, message.UserID);
            Assert.Equal(25, message.Bill);
            Assert.Equal(dt1, message.CreatedAt);
            Assert.Equal(dt2, message.Reserved);
        }

        [Fact]
        public void PaidOrderMessage_Test()
        {
            DateTime dt = DateTime.Now.AddMinutes(2);

            PaidOrderMessage message = new()
            {
                OrderID = Guid.NewGuid(),
                Payload = "Test",
                Reserved = dt
            };

            Assert.NotEmpty(message.OrderID.ToString());
            Assert.Equal("Test", message.Payload);
            Assert.Equal(dt, message.Reserved);

            Assert.Throws<ArgumentNullException>(() => message.Payload = null!);
        }
    }
}
