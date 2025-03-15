using Main.Helpers;
using HseBankLibrary.Models;
using MenuLibrary;
using Microsoft.Extensions.DependencyInjection;

namespace Main
{
    public class Program
    {
        /// <summary>
        /// Точка входа программы.
        /// </summary>
        /// <param name="args">Аргумменты командной строки.</param>
        public static void Main()
        {
            // Чтение конфига добавление параметров через консоль.
            Configuration cfg = Utils.GetConfiguration();

            using ServiceProvider? provider = Utils.Initialize(cfg);
            if (provider == null) return;

            // Перехват Ctrl+C.
            Console.CancelKeyPress += (_, _) => Utils.Abort(provider);

            // Создание и запуск меню.
            Menu menu = Utils.CreateMainMenu(provider);
            menu.Run();
        }
    }
}
