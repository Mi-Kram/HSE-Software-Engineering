using HseBankLibrary.Models;

namespace HseBankLibrary.Tests.Models
{
    public class ConfigurationTests
    {
        [Fact]
        public void ValidPropertiesCheck()
        {
            Assert.Throws<ArgumentException>(() => new Configuration() { StoragePath = null! });
            Assert.Throws<ArgumentException>(() => new Configuration() { BankAccountsFileName = "" });
            Assert.Throws<ArgumentException>(() => new Configuration() { CategoriesFileName = "  " });
            Assert.Throws<ArgumentException>(() => new Configuration() { OperationsFileName = null! });

            Configuration cfg = new();
            Assert.NotEmpty(cfg.StoragePath);
            Assert.NotEmpty(cfg.BankAccountsFileName);
            Assert.NotEmpty(cfg.CategoriesFileName);
            Assert.NotEmpty(cfg.OperationsFileName);
        }
    }
}
