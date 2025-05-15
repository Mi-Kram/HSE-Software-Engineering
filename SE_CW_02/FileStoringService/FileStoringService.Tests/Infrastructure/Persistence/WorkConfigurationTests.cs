using FileStoringService.Domain.Entities;
using FileStoringService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FileStoringService.Tests.Infrastructure.Persistence
{
    public class WorkConfigurationTests
    {

        [Fact]
        public void WorkInfo_Table_ShouldBeConfiguredCorrectly()
        {
            DbContextOptionsBuilder<WorkDbContext> builder = new DbContextOptionsBuilder<WorkDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase"); // Используем in-memory провайдер
            WorkDbContext context = new(builder.Options);
            IEntityType? entityType = context.Model.FindEntityType(typeof(WorkInfo));

            Assert.NotNull(entityType);

            // Проверка имени таблицы
            Assert.Equal("works", entityType.GetTableName());

            // Проверка наличия ключа
            Assert.Contains(entityType.FindPrimaryKey()?.Properties ?? [], p => p.Name == nameof(WorkInfo.ID));

            // Проверка конфигурации свойства ID
            IProperty? idProperty = entityType.FindProperty(nameof(WorkInfo.ID));
            Assert.NotNull(idProperty);
            Assert.Equal("id", idProperty.GetColumnName());

            // Проверка конфигурации свойства UserID
            IProperty? userIdProperty = entityType.FindProperty(nameof(WorkInfo.UserID));
            Assert.NotNull(userIdProperty);
            Assert.Equal("user_id", userIdProperty.GetColumnName());

            // Проверка конфигурации свойства Hash
            IProperty? hashProperty = entityType.FindProperty(nameof(WorkInfo.Hash));
            Assert.NotNull(hashProperty);
            Assert.Equal("hash", hashProperty.GetColumnName());
            Assert.Equal(64, hashProperty.GetMaxLength());

            // Проверка наличия индекса для свойства Hash
            Assert.Contains(entityType.GetIndexes(), i => i.Properties.Any(p => p.Name == nameof(WorkInfo.Hash)));

            // Проверка конфигурации свойства Uploaded
            var uploadedProperty = entityType.FindProperty(nameof(WorkInfo.Uploaded));
            Assert.NotNull(uploadedProperty);
            Assert.Equal("uploaded", uploadedProperty.GetColumnName());
        }
    }
}
