using HseBankLibrary.Exceptions;
using HseBankLibrary.Models.Domain;
using HseBankLibrary.Models.Domain.DTO;
using HseBankLibrary.Storage;
using HseBankLibrary.Storage.AutoIncrement;
using Moq;

namespace HseBankLibrary.Tests.Storage
{
    public class DatabaseTests
    {
        private static Database GetDefaultDatabase()
        {
            UlongAutoIncrement ainc = new();
            MemoryStorage<ulong, BankAccount, BankAccountDTO> accounts = new(ainc);
            var cachedAccounts = CachedStorage<ulong, BankAccount, BankAccountDTO>.Create(accounts, ainc);

            UlongAutoIncrement cinc = new();
            MemoryStorage<ulong, Category, CategoryDTO> categories = new(cinc);
            var cachedCategories = CachedStorage<ulong, Category, CategoryDTO>.Create(categories, cinc);

            StringAutoIncrement oinc = new();
            MemoryStorage<string, Operation, OperationDTO> operations = new(oinc);
            var cachedOperations = CachedStorage<string, Operation, OperationDTO>.Create(operations, oinc);

            return Database.CreateBuilder()
                .SetBankAccountsStorage(cachedAccounts)
                .SetCategoriesStorage(cachedCategories)
                .SetOperationsStorage(cachedOperations)
                .Build();
        }

        [Fact]
        public void Constructor_ThrowException()
        {
            var defaultDB = GetDefaultDatabase();

            var builder = new Database.Builder();
            builder = Database.CreateBuilder();

            Assert.Throws<InvalidDataException>(() => builder.Build());
            builder.SetBankAccountsStorage(defaultDB.BankAccounts);

            Assert.Throws<InvalidDataException>(() => builder.Build());
            builder.SetCategoriesStorage(defaultDB.Categories);

            Assert.Throws<InvalidDataException>(() => builder.Build());
            builder.SetOperationsStorage(defaultDB.Operations);

            Assert.NotNull(builder.Build());

            defaultDB.Operations.Add(new Operation());
            Assert.Throws<DatabaseEntitiesMismatchException>(() => builder.Build());
        }

        [Fact]
        public void SaveDataTests()
        {
            var db = GetDefaultDatabase();

            db.BankAccounts.Add(new BankAccount());
            db.Categories.Add(new Category());
            db.Operations.Add(new Operation());

            db.SaveData();
        }
    }
}
