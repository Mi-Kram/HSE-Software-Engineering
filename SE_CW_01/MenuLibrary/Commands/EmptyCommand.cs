namespace MenuLibrary.Commands
{
    /// <summary>
    /// Команда-заглушка, ничего не делает.
    /// </summary>
    public class EmptyCommand : ICommand<CommandArguments>
    {
        private static EmptyCommand? instance = null;
        private static readonly object instanceLock = new();

        public static EmptyCommand Instance
        {
            get
            {
                lock (instanceLock)
                {
                    instance ??= new EmptyCommand();
                }

                return instance;
            }
        }
        
        private EmptyCommand()
        { }

        public void Execute(CommandArguments args)
        { }
    }
}
