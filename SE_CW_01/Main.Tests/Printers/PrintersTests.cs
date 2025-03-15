using HseBankLibrary.Models.Domain;
using Main.Printers;

namespace Main.Tests.Printers
{
    [Collection("Console")]
    public class PrintersTests
    {
        [Fact]
        public void BankAccountPrinterTest()
        {
            TextWriter consoleWriter = Console.Out;
            using StringWriter sw = new();
            Console.SetOut(sw);

            BankAccount account = new() { ID = 5, Name = "Test", Balance = 3.4M };
            BankAccountPrinter printer = new();

            printer.Print(null!);
            printer.Print(account);

            string result = sw.ToString();
            Console.SetOut(consoleWriter);

            string expected = $"Счет: NULL{Environment.NewLine}ID: {account.ID}, Название: {account.Name}, Баланс: {account.Balance}{Environment.NewLine}";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CategoryPrinterTest()
        {
            TextWriter consoleWriter = Console.Out;
            using StringWriter sw = new();
            Console.SetOut(sw);

            Category category = new() { ID = 5, Name = "Test" };
            CategoryPrinter printer = new();

            printer.Print(null!);
            printer.Print(category);

            string result = sw.ToString();
            Console.SetOut(consoleWriter);

            string expected = $"Категория: NULL{Environment.NewLine}ID: {category.ID}, Название: {category.Name}{Environment.NewLine}";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void OperationPrinterTest()
        {
            TextWriter consoleWriter = Console.Out;
            using StringWriter sw = new();
            Console.SetOut(sw);

            Operation operation = new() { 
                ID = "123", 
                BankAccountID = 5,
                CategoryID = 7,
                Amount = 7.3M,
                Date = DateTime.Now,
                IsIncome = true,
                Description = "Test"
            };
            OperationPrinter printer = new();

            printer.Print(null!);
            printer.Print(operation);

            string result = sw.ToString();
            Console.SetOut(consoleWriter);

            using StringWriter s = new();
            s.WriteLine("Категория: NULL");
            s.WriteLine($"ID: {operation.ID}");
            s.Write($"Тип: {(operation.IsIncome ? "доход" : "расход")}, ");
            s.Write($"ID счёта: {operation.BankAccountID}, ");
            s.WriteLine($"ID категории: {operation.CategoryID}");

            s.Write($"Сумма: {operation.Amount}, ");
            s.WriteLine($"Дата: {operation.Date}");

            s.WriteLine($"Комментарий: {operation.Description}");

            string expected = s.ToString();
            Assert.Equal(expected, result);
        }
    }
}
