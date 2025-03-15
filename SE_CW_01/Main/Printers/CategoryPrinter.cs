using HseBankLibrary.Models.Domain;

namespace Main.Printers
{
    /// <summary>
    /// Вывод информации о категории.
    /// </summary>
    public class CategoryPrinter : IPrinter<Category>
    {

        public void Print(Category item)
        {
            if (item == null)
            {
                Console.WriteLine("Категория: NULL");
                return;
            }

            Console.WriteLine($"ID: {item.ID}, Название: {item.Name}");
        }
    }
}
