using MenuLibrary;
using MenuLibrary.Commands;

namespace Main.Controllers.Common
{
    /// <summary>
    /// Общий обработчик выходаиз подменю.
    /// </summary>
    public class ExitController(Menu menu) : ICommand<CommandArguments>
    {
        private readonly Menu menu = menu ?? throw new ArgumentNullException(nameof(menu));

        public void Execute(CommandArguments args)
        {
            menu.Stop();
            args.AskForEnter = false;
        }
    }
}
