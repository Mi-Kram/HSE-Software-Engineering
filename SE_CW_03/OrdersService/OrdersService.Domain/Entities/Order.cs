namespace OrdersService.Domain.Entities
{
    public class Order
    {
		private Guid id;
		private int user_id;
		private DateTime created_at;
		private decimal bill;
		private OrderStatus status;

		public Guid ID
		{
			get => id;
			set => id = value;
        }

		public int UserID
        {
			get => user_id;
			set => user_id = value;
        }

		public DateTime CreatedAt
        {
			get => created_at;
			set => created_at = value;
        }

		public decimal Bill
        {
			get => bill;
			set => bill = 0 <= value ? value : throw new ArgumentException("Счет на оплату не может быть отрицательным", nameof(Bill));
        }

		public OrderStatus Status
		{
			get => status;
			set => status = value;
        }
	}
}
