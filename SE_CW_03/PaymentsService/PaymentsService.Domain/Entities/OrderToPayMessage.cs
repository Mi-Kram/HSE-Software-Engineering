namespace PaymentsService.Domain.Entities
{
    public class OrderToPayMessage
    {
        private Guid orderID;
        private int userID;
        private decimal bill;
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

        public decimal Bill
        {
            get => bill;
            set => bill = value;
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
