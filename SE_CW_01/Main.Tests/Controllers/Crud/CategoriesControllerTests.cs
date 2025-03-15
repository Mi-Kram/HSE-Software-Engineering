using HseBankLibrary.Factories;
using Main.Models.Timing;
using HseBankLibrary.Storage;
using Main.Controllers.Crud.Categories;
using HseBankLibrary.Models.Domain;
using Main.Printers;
using MenuLibrary;
using Microsoft.Extensions.DependencyInjection;

namespace Main.Tests.Controllers.Crud
{
    [Collection("Console")]
    public class CategoriesControllerTests
    {
        [Fact]
        public void GeneralTests()
        {
            ServiceCollection services = new();
            services.AddSingleton(Helper.GetDefaultDatabase());
            services.AddSingleton(new TimingReport());
            services.AddSingleton<CategoryFactory>();
            using ServiceProvider provider = services.BuildServiceProvider();

            TextWriter consoleOut = Console.Out;
            TextReader consoleIn = Console.In;

            using StringWriter sw = new();
            using StringReader sr = new("5\n");

            Console.SetOut(sw);
            Console.SetIn(sr);

            GeneralController cmd = new(provider);
            cmd.Execute(new MenuLibrary.Commands.CommandArguments());

            Console.SetOut(consoleOut);
            Console.SetIn(consoleIn);

            using StringWriter s = new();
            s.WriteLine("Выберите действие");
            s.WriteLine("1. Просмотр категорий");
            s.WriteLine("2. Создать категорию");
            s.WriteLine("3. Удалить категорию");
            s.WriteLine("4. Редактировать категорию");
            s.WriteLine("5. Назад");
            s.WriteLine("Выберите действие: ");
            s.WriteLine();

            string expected = s.ToString();
            Assert.Equal(expected, sw.ToString());
        }

        [Fact]
        public void CreateTests()
        {
            Database db = Helper.GetDefaultDatabase();

            ServiceCollection services = new();
            services.AddSingleton(db);
            services.AddSingleton<CategoryFactory>();
            using ServiceProvider provider = services.BuildServiceProvider();

            TextWriter consoleOut = Console.Out;
            TextReader consoleIn = Console.In;

            using StringWriter sw = new();
            using StringReader sr = new("NewCategory\n");

            Console.SetOut(sw);
            Console.SetIn(sr);

            CreateController cmd = new(provider);
            cmd.Execute(new MenuLibrary.Commands.CommandArguments());

            Console.SetOut(consoleOut);
            Console.SetIn(consoleIn);

            using StringWriter s = new();
            s.Write("Введите название категории (пустая строка - отмена): ");
            s.WriteLine("Категория успешно добавлена");

            string expected = s.ToString();
            Assert.Equal(expected, sw.ToString());

            var categories = db.Categories.GetAll();
            Assert.Single(categories);
            Assert.Equal("NewCategory", categories.First().Name);
        }

        [Fact]
        public void DeleteTests()
        {
            Database db = Helper.GetDefaultDatabase();
            db.Categories.Add(new Category() { Name = "DeleteMe" });

            ServiceCollection services = new();
            services.AddSingleton(db);
            using ServiceProvider provider = services.BuildServiceProvider();

            TextWriter consoleOut = Console.Out;
            TextReader consoleIn = Console.In;

            using StringWriter sw = new();
            using StringReader sr = new("0\n");

            Console.SetOut(sw);
            Console.SetIn(sr);

            DeleteController cmd = new(provider);
            cmd.Execute(new MenuLibrary.Commands.CommandArguments());

            Console.SetOut(consoleOut);
            Console.SetIn(consoleIn);

            using StringWriter s = new();
            s.Write("Введите ID категории (пустая строка - отмена): ");
            s.WriteLine("Объект успешно удалён");

            string expected = s.ToString();
            Assert.Equal(expected, sw.ToString());

            var categories = db.Categories.GetAll();
            Assert.Empty(categories);
        }

        [Fact]
        public void UpdateTests()
        {
            Database db = Helper.GetDefaultDatabase();
            db.Categories.Add(new Category() { Name = "UpdateMe" });

            ServiceCollection services = new();
            services.AddSingleton(db);
            using ServiceProvider provider = services.BuildServiceProvider();

            TextWriter consoleOut = Console.Out;
            TextReader consoleIn = Console.In;

            using StringWriter sw = new();
            using StringReader sr = new("0\nUpdated\n");

            Console.SetOut(sw);
            Console.SetIn(sr);

            UpdateController cmd = new(provider);
            cmd.Execute(new MenuLibrary.Commands.CommandArguments());

            Console.SetOut(consoleOut);
            Console.SetIn(consoleIn);

            using StringWriter s = new();
            s.Write("Введите ID категории (пустая строка - отмена): ");
            s.Write("Введите новое название категории (пустая строка - отмена): ");
            s.WriteLine("Категория успешно обновлена");

            string expected = s.ToString();
            Assert.Equal(expected, sw.ToString());

            var categories = db.Categories.GetAll();
            Assert.Single(categories);
            Assert.Equal("Updated", categories.First().Name);
        }

        [Fact]
        public void PrintTests()
        {
            Database db = Helper.GetDefaultDatabase();
            db.Categories.Add(new Category() { Name = "Test1" });
            db.Categories.Add(new Category() { Name = "Test2" });
            db.Categories.Add(new Category() { Name = "Test3" });

            ServiceCollection services = new();
            services.AddSingleton(db);
            using ServiceProvider provider = services.BuildServiceProvider();

            TextWriter consoleOut = Console.Out;
            using StringWriter sw = new();
            Console.SetOut(sw);

            PrintController cmd = new(provider, new CategoryPrinter());
            cmd.Execute(new MenuLibrary.Commands.CommandArguments());

            Console.SetOut(consoleOut);

            using StringWriter s = new();
            s.WriteLine($"ID: 0, Название: Test1");
            s.WriteLine($"ID: 1, Название: Test2");
            s.WriteLine($"ID: 2, Название: Test3");

            string expected = s.ToString();
            Assert.Equal(expected, sw.ToString());
        }
    }
}
