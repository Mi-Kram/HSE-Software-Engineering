using HseBankLibrary.Models.Domain;

namespace Main.Printers
{
    /// <summary>
    /// Вывод информации о счете.
    /// </summary>
    public class BankAccountPrinter : IPrinter<BankAccount>
    {
        public void Print(BankAccount item)
        {
            if (item == null)
            {
                Console.WriteLine("Счет: NULL");
                return;
            }

            Console.WriteLine($"ID: {item.ID}, Название: {item.Name}, Баланс: {item.Balance}");
        }

    }
}
