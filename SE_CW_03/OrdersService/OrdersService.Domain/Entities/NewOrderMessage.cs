namespace OrdersService.Domain.Entities
{
    public class NewOrderMessage
    {
        private Guid orderID;
        private int userID;
        private string payload = string.Empty;
        private DateTime createdAt;
        private DateTime reserved;

        public Guid OrderID
        {
            get => orderID;
            set => orderID = value;
        }

        public int UserID
        {
            get => userID;
            set => userID = value;
        }

        public string Payload
        {
            get => payload;
            set => payload = value ?? throw new ArgumentNullException(nameof(Payload));
        }

        public DateTime CreatedAt
        {
            get => createdAt;
            set => createdAt = value;
        }

        public DateTime Reserved
        {
            get => reserved;
            set => reserved = value;
        }
    }
}
