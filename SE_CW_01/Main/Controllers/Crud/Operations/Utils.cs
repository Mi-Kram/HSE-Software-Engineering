using HseBankLibrary.Models.Domain;

namespace Main.Controllers.Crud.Operations
{
    internal static class Utils
    {
        /// <summary>
        /// Чтение id типа Ulong.
        /// </summary>
        /// <param name="promt">Запрос на ввод.</param>
        /// <param name="lst">Список существующих сущностей.</param>
        /// <returns>ID или NULL.</returns>
        public static ulong? ReadUlong(string promt, IEnumerable<IEntity<ulong>> lst)
        {
            // Первое чтение данных.
            Console.Write(promt);
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) return null;

            // Пока данные введены некорректно - повторять ввод.
            ulong id;
            while (!ulong.TryParse(input, out id) || !lst.Any(x => x.ID == id))
            {
                Console.WriteLine($"Неверный ввод!\n");
                Console.Write(promt);
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) return null;
            }

            return id;
        }

        /// <summary>
        /// Чтение типа операции.
        /// </summary>
        /// <param name="promt">Запрос на ввод.</param>
        /// <param name="positive">Значение дохода.</param>
        /// <param name="negative">Значение расхода.</param>
        /// <returns>Тип операции или NULL.</returns>
        public static bool? ReadOperationType(string promt, int positive, int negative)
        {
            // Первое чтение данных.
            Console.Write(promt);
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) return null;

            // Пока данные введены некорректно - повторять ввод.
            int num;
            while (!int.TryParse(input, out num) || (num != positive && num != negative))
            {
                Console.WriteLine($"Неверный ввод!\n");
                Console.Write(promt);
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) return null;
            }

            return num == positive;
        }

        /// <summary>
        /// Чтение суммы операции.
        /// </summary>
        /// <param name="promt">Запрос на ввод.</param>
        /// <returns>Сумма операции или NULL.</returns>
        public static decimal? ReadAmount(string promt)
        {
            // Первое чтение данных.
            Console.Write(promt);
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) return null;

            // Пока данные введены некорректно - повторять ввод.
            decimal amount;
            while (!decimal.TryParse(input, out amount) || amount <= 0)
            {
                Console.WriteLine($"Неверный ввод!\n");
                Console.Write(promt);
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) return null;
            }

            return amount;
        }

    }
}
