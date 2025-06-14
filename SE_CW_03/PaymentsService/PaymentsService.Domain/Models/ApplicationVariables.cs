namespace PaymentsService.Domain.Models
{
    public class ApplicationVariables
    {
        public const string DB_CONNECTION = "PaymentsService.DB_CONNECTION";
        public const string KAFKA = "PaymentsService.KAFKA";
        public const string TOPIC_NEW_ORDER = "PaymentsService.TOPIC_NEW_ORDER";
        public const string TOPIC_PAID_ORDER = "PaymentsService.TOPIC_PAID_ORDER";
    }
}
