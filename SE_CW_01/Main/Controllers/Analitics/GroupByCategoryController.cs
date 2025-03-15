using HseBankLibrary.Models.Domain;
using HseBankLibrary.Storage;
using MenuLibrary.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Controllers.Analitics
{
    /// <summary>
    /// Группировка доходов и расходов по категориям.
    /// </summary>
    public class GroupByCategoryController(ServiceProvider provider) : ICommand<CommandArguments>
    {
        private readonly Database db = provider.GetRequiredService<Database>();

        public void Execute(CommandArguments args)
        {
            // Получение всех операций и групировка их по категориям.
            Dictionary<ulong, List<Operation>> data = db.Operations.GetAll().OrderBy(x => x.Date)
                .GroupBy(x => x.CategoryID)
                .ToDictionary(x => x.Key, x => x.ToList());

            if (data.Count == 0)
            {
                Console.WriteLine("Нет операций для вывода");
                return;
            }

            // Получение всех категорий.
            Dictionary<ulong, Category> categpries = db.Categories.GetAll().ToDictionary(x => x.ID);
            
            // Вывод операций для каждой категории.
            foreach (var item in data)
            {
                string name = categpries.TryGetValue(item.Key, out Category? ctg) ? ctg.Name : item.Key.ToString();
                Console.WriteLine($"Категория: {name}");
                PrintOperations(item.Value);
                Console.WriteLine();
            }
        }

        private static void PrintOperations(List<Operation> operations)
        {
            foreach (Operation op in operations)
            {
                string sign = op.IsIncome ? "+" : "-";
                Console.WriteLine($" - {op.Date:dd.MM.yyyyy HH:mm:ss}: {sign}{op.Amount}");
            }
        }
    }
}
