using MenuLibrary.Commands;

namespace MenuLibrary
{
    /// <summary>
    /// Меню.
    /// </summary>
    public class Menu
    {
        private readonly List<MenuItem> menuItems = [];
        private bool isRunning = false;
        private string title = string.Empty;

        /// <summary>
        /// Чистить консоль при каждом выводе меню.
        /// </summary>
        public bool ClearConsole { get; set; } = true;

        /// <summary>
        /// Заголовок меню.
        /// </summary>
        public string Title
        {
            get => title;
            set => title = value ?? throw new ArgumentNullException(nameof(Title));
        }

        /// <summary>
        /// Добавление пункта меню.
        /// </summary>
        /// <param name="item">Новый пункт меню.</param>
        public void Add(MenuItem item)
        {
            if (item == null) return;
            menuItems.Add(item);
        }

        /// <summary>
        /// Вставка пункта меню.
        /// </summary>
        /// <param name="pos">Индекс пункта меню.</param>
        /// <param name="item">Новый пункт меню.</param>
        public void Insert(int pos, MenuItem item)
        {
            if (item == null || pos < 0 || pos > menuItems.Count) return;
            menuItems.Insert(pos, item);
        }

        /// <summary>
        /// Удаление пункта меню.
        /// </summary>
        /// <param name="item">Пункт меню для удаления.</param>
        public void Delete(MenuItem item)
        {
            menuItems.Remove(item);
        }

        /// <summary>
        /// Запуск меню.
        /// </summary>
        /// <param name="cycle">Повторение запуска меню</param>
        public void Run(bool cycle = true)
        {
            isRunning = true;  // флаг остановки цикла.
            
            while (isRunning)
            {
                // Вывод меню.
                if (ClearConsole) Console.Clear();
                int total = PrintMenu();

                // Если в меню нет пунктов - прекратить работу меню.
                if (total == 0)
                {
                    Console.WriteLine("В меню нет пунктов!");
                    isRunning = false;
                    continue;
                }

                // Выбор действия.
                Console.Write("Выберите действие: ");
                MenuItem? item = SelectItem(total);
                Console.WriteLine();

                if (item == null)
                {
                    Console.WriteLine("Неверный ввод!\n");
                    if (ClearConsole) AskForEnter();
                    continue;
                }

                // Запуск команды.
                CommandArguments arguments = new(true);
                item.Execute(arguments);

                // Запрос нажатия Enter, если требуется.
                if (arguments.AskForEnter) AskForEnter();
                Console.WriteLine();

                // Если меню должно было выполниться один раз - изменить флаг остановки.
                if (!cycle) isRunning = false;
            }
        }

        /// <summary>
        /// Запрос нажатия Enter.
        /// </summary>
        private static void AskForEnter()
        {
            Console.WriteLine("\n\nДля продолжения нажмите Enter");
            Console.CursorVisible = false;
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) ;
            Console.CursorVisible = true;
        }

        /// <summary>
        /// Вывод меню.
        /// </summary>
        /// <returns></returns>
        private int PrintMenu()
        {
            int pos = 0;

            if (!string.IsNullOrWhiteSpace(Title)) Console.WriteLine(Title);

            foreach (MenuItem item in menuItems)
            {
                Console.WriteLine($"{++pos}. {item.Title}");
            }

            return pos;
        }

        /// <summary>
        /// Запрос выбора пункта меню.
        /// </summary>
        /// <param name="total"></param>
        /// <returns></returns>
        private MenuItem? SelectItem(int total)
        {
            if (!int.TryParse(Console.ReadLine(), out int pos) || pos <= 0 || pos > total) return null;

            int i = 0;
            foreach (MenuItem item in menuItems)
            {
                if (++i == pos) return item;
            }

            return null;
        }

        /// <summary>
        /// Остановить меню.
        /// </summary>
        public void Stop()
        {
            isRunning = false;
        }
    }
}
