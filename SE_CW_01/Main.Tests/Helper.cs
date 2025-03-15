using HseBankLibrary.Models.Domain.DTO;
using HseBankLibrary.Models.Domain;
using HseBankLibrary.Storage.AutoIncrement;
using HseBankLibrary.Storage;

namespace Main.Tests
{
    internal static class Helper
    {
        public static Database GetDefaultDatabase()
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
    }
}
