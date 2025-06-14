namespace OrdersService.Domain.DTO
{
    public class OrderDTO
    {
        private decimal bill;

        public int UserID { get; set; }
        public decimal Bill
        {
            get => bill;
            set => bill = 0 <= value ? value : throw new ArgumentException("Счет на оплату не может быть отрицательным", nameof(Bill));
        }
        public DateTime CreatedAt { get; set; }
    }
}
