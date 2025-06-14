using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentsService.Infrastructure.DTO
{
    public class OrderIdDTO
    {
        [Column("order_id")]
        public Guid OrderID { get; set; }
    }
}
