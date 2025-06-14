namespace PaymentsService.Domain.Entities
{
    public class PaidOrderMessage
    {
        private Guid orderID;
        private string payload = string.Empty;
        private DateTime reserved;

        public Guid OrderID
        {
            get => orderID;
            set => orderID = value;
        }

        public string Payload
        {
            get => payload;
            set => payload = value ?? throw new ArgumentNullException(nameof(Payload));
        }

        public DateTime Reserved
        {
            get => reserved;
            set => reserved = value;
        }
    }
}
