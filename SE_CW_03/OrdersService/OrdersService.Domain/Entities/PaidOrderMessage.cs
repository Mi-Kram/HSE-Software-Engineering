namespace OrdersService.Domain.Entities
{
    public class PaidOrderMessage
    {
        private Guid orderID;
        private OrderStatus status;
        private DateTime reserved;

        public Guid OrderID
        {
            get => orderID;
            set => orderID = value;
        }

        public OrderStatus Status
        {
            get => status;
            set => status = value;
        }

        public DateTime Reserved
        {
            get => reserved;
            set => reserved = value;
        }
    }
}