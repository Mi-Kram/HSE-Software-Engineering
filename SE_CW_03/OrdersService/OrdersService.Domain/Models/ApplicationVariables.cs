namespace OrdersService.Domain.Models
{
    public class ApplicationVariables
    {
        public const string DB_CONNECTION = "OrdersService.DB_CONNECTION";
        public const string KAFKA = "OrdersService.KAFKA";
        public const string TOPIC_NEW_ORDER = "OrdersService.TOPIC_NEW_ORDER";
        public const string TOPIC_PAID_ORDER = "OrdersService.TOPIC_PAID_ORDER";
    }
}
