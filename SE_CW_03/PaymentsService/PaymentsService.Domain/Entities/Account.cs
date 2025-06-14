namespace PaymentsService.Domain.Entities
{
    public class Account
    {
        private int userID;
        private string caption = string.Empty;
        private decimal balance;

        public int UserID
        {
            get => userID;
            set => userID = value;
        }

        public string Caption
        {
            get => caption;
            set => caption = value ?? throw new ArgumentNullException(nameof(Caption));
        }

        public decimal Balance
        {
            get => balance;
            set => balance = value;
        }
    }
}
