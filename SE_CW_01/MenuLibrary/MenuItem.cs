using MenuLibrary.Commands;

namespace MenuLibrary
{
    /// <summary>
    /// Пункт меню.
    /// </summary>
    public class MenuItem
    {
		private string title = "";
		private ICommand<CommandArguments> command = EmptyCommand.Instance;

        /// <summary>
        /// Название пункта меню.
        /// </summary>
		public string Title
		{
			get => title;
			set => title = value ?? throw new ArgumentNullException(nameof(Title));
		}

        /// <summary>
        /// Выполняемая команда.
        /// </summary>
		public ICommand<CommandArguments> Command
        {
			get => command;
			set => command = value ?? throw new ArgumentNullException(nameof(Command));
		}

        public MenuItem()
        { }

        public MenuItem(string title)
        {
            Title = title;
        }

        public MenuItem(string title, ICommand<CommandArguments> command)
        {
            Title = title;
            Command = command;
        }

        public void Execute(CommandArguments args)
        {
            command.Execute(args);
        }
    }
}
