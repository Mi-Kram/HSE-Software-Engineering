using MenuLibrary.Commands;

namespace Main.Models.Timing
{
    /// <summary>
    /// Декоратор для засекания времени исполнения обработчика меню.
    /// </summary>
    public class TimingDecorator(ICommand<CommandArguments> command, string label, TimingReport reporter) : ICommand<CommandArguments>
    {
        /// <summary>
        /// Метка обработчика.
        /// </summary>
        private readonly string label = label ?? throw new ArgumentNullException(nameof(label));
        
        /// <summary>
        /// Настоящий обработчик.
        /// </summary>
        private readonly ICommand<CommandArguments> command = command ?? throw new ArgumentNullException(nameof(command));

        /// <summary>
        /// Зранилище времени работы обработчика.
        /// </summary>
        private readonly TimingReport reporter = reporter ?? throw new ArgumentNullException(nameof(reporter));

        public void Execute(CommandArguments args)
        {
            // Засекание времени исполнения.
            DateTime start = DateTime.Now;
            command.Execute(args);
            DateTime end = DateTime.Now;

            // Добавление результата.
            reporter.Add(label, end - start);
        }
    }
}
