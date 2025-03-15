using HseBankLibrary.Exceptions;
using HseBankLibrary.Storage;
using MenuLibrary;
using MenuLibrary.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Controllers
{
    /// <summary>
    /// Обработчик выхода из главного меню.
    /// </summary>
    public class SaveExitController(ServiceProvider provider, Menu menu) : ICommand<CommandArguments>
    {
        private readonly Menu menu = menu ?? throw new ArgumentNullException(nameof(menu));
        private readonly Database db = provider.GetRequiredService<Database>();

        public void Execute(CommandArguments args)
        {
            // Завершение главного меню.
            menu.Stop();

            try
            {
                // Сохранение данных.
                db.SaveData();
                Console.WriteLine("Прогрмма успешно завершена.");
            }
            catch (OnSaveDatabaseException ex)
            {
                foreach (string storage in ex.NameOfStorages)
                {
                    string storageName = storage switch
                    {
                        nameof(db.BankAccounts) => "счетов",
                        nameof(db.Categories) => "категорий",
                        nameof(db.Operations) => "операций",
                        _ => string.Empty
                    };
                    Console.WriteLine($"Не удалось сохранить репозиторий {storageName}");
                }

                Console.WriteLine("\nПрограмма завершена.");
            }
        }
    }
}
