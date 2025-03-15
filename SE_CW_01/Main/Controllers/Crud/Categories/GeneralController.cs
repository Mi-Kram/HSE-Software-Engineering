﻿using Main.Models.Timing;
using Main.Controllers.Common;
using Main.Printers;
using MenuLibrary;
using MenuLibrary.Commands;
using Microsoft.Extensions.DependencyInjection;
using Main.Controllers.Crud.BankAccounts;

namespace Main.Controllers.Crud.Categories
{
    /// <summary>
    /// Меню действий с категориями.
    /// </summary>
    public class GeneralController(ServiceProvider provider) : ICommand<CommandArguments>
    {
        private readonly ServiceProvider provider = provider;
        private readonly TimingReport reporter = provider.GetRequiredService<TimingReport>();

        public void Execute(CommandArguments args)
        {
            Menu menu = CreateCrudMenu();
            menu.Run();
            args.AskForEnter = false;
        }

        private Menu CreateCrudMenu()
        {
            Menu menu = new()
            {
                Title = "Выберите действие",
                ClearConsole = false
            };

            TimingDecorator print = new(new PrintController(provider, new CategoryPrinter()), "CRUD.Categories.Print", reporter);
            TimingDecorator create = new(new CreateController(provider), "CRUD.Categories.Create", reporter);
            TimingDecorator delete = new(new DeleteController(provider), "CRUD.Categories.Delete", reporter);
            TimingDecorator update = new(new UpdateController(provider), "CRUD.Categories.Update", reporter);

            menu.Add(new MenuItem("Просмотр категорий", print));
            menu.Add(new MenuItem("Создать категорию", create));
            menu.Add(new MenuItem("Удалить категорию", delete));
            menu.Add(new MenuItem("Редактировать категорию", update));
            menu.Add(new MenuItem("Назад", new ExitController(menu)));

            return menu;
        }

    }
}
