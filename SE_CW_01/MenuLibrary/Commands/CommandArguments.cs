namespace MenuLibrary.Commands
{
    /// <summary>
    /// Параметр пункта меню.
    /// </summary>
    public class CommandArguments
    {
        /// <summary>
        /// Запрашивать нажатие Enter после выполнения пункта меню.
        /// </summary>
        public bool AskForEnter { get; set; } = true;

        public CommandArguments() 
        { }

        public CommandArguments(bool askForEnter)
        {
            AskForEnter = askForEnter;
        }
    }
}
