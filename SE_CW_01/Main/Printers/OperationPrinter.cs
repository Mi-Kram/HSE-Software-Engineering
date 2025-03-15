using HseBankLibrary.Models.Domain;

namespace Main.Printers
{
    /// <summary>
    /// Вывод информации об операции.
    /// </summary>
    public class OperationPrinter : IPrinter<Operation>
    {
        public void Print(Operation item)
        {
            if (item == null)
            {
                Console.WriteLine("Категория: NULL");
                return;
            }

            Console.WriteLine($"ID: {item.ID}");
            Console.Write($"Тип: {(item.IsIncome ? "доход" : "расход")}, ");
            Console.Write($"ID счёта: {item.BankAccountID}, ");
            Console.WriteLine($"ID категории: {item.CategoryID}");

            Console.Write($"Сумма: {item.Amount}, ");
            Console.WriteLine($"Дата: {item.Date}");

            Console.WriteLine($"Комментарий: {item.Description}");
        }
    }
}
