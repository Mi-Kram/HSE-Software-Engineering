namespace OrderStatusChangeNotifier.Domain.Models
{
    public class ApplicationVariables
    {
        public const string KAFKA = "OrderStatusChangeNotifier.KAFKA";
        public const string TOPIC_NEW_ORDER = "OrderStatusChangeNotifier.TOPIC_NEW_ORDER";
        public const string TOPIC_PAID_ORDER = "OrderStatusChangeNotifier.TOPIC_PAID_ORDER";
    }
}
