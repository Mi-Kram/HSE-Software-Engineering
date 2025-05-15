using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using FileAnalysisService.Infrastructure.Persistence.Context;
using FileAnalysisService.Domain.Entities;

namespace FileAnalysisService.Tests.Infrastructure.Persistence
{
    public class ApplicationDbContextTests
    {
        [Fact]
        public void AnalyzeReportConfiguration_Tests()
        {
            DbContextOptionsBuilder<ApplicationDbContext> builder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AnalyzeReportConfiguration_Tests");
            ApplicationDbContext context = new(builder.Options);
            IEntityType? entityType = context.Model.FindEntityType(typeof(AnalyzeReport));

            Assert.NotNull(entityType);

            // Проверка имени таблицы
            Assert.Equal("analyse-reports", entityType.GetTableName());

            // Проверка наличия ключа
            Assert.Contains(entityType.FindPrimaryKey()?.Properties ?? [], p => p.Name == nameof(AnalyzeReport.WorkID));

            // Проверка конфигурации свойства ID
            IProperty? idProperty = entityType.FindProperty(nameof(AnalyzeReport.WorkID));
            Assert.NotNull(idProperty);
            Assert.Equal("work_id", idProperty.GetColumnName());

            // Проверка конфигурации свойства UserID
            IProperty? paragraphsProperty = entityType.FindProperty(nameof(AnalyzeReport.Paragraphs));
            Assert.NotNull(paragraphsProperty);
            Assert.Equal("paragraphs", paragraphsProperty.GetColumnName());

            // Проверка конфигурации свойства Hash
            IProperty? wordsProperty = entityType.FindProperty(nameof(AnalyzeReport.Words));
            Assert.NotNull(wordsProperty);
            Assert.Equal("words", wordsProperty.GetColumnName());

            // Проверка конфигурации свойства Hash
            IProperty? numbersProperty = entityType.FindProperty(nameof(AnalyzeReport.Numbers));
            Assert.NotNull(numbersProperty);
            Assert.Equal("numbers", numbersProperty.GetColumnName());

            // Проверка конфигурации свойства Hash
            IProperty? symbolsProperty = entityType.FindProperty(nameof(AnalyzeReport.Symbols));
            Assert.NotNull(symbolsProperty);
            Assert.Equal("symbols", symbolsProperty.GetColumnName());
        }

        [Fact]
        public void ComparisonReportConfiguration_Tests()
        {
            DbContextOptionsBuilder<ApplicationDbContext> builder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ComparisonReportConfiguration_Tests");
            ApplicationDbContext context = new(builder.Options);
            IEntityType? entityType = context.Model.FindEntityType(typeof(ComparisonReport));

            Assert.NotNull(entityType);

            // Проверка имени таблицы
            Assert.Equal("comparison-reports", entityType.GetTableName());

            // Проверка наличия ключа
            Assert.Contains(entityType.FindPrimaryKey()?.Properties ?? [], p => p.Name == nameof(ComparisonReport.Work1ID));
            Assert.Contains(entityType.FindPrimaryKey()?.Properties ?? [], p => p.Name == nameof(ComparisonReport.Work2ID));

            // Проверка конфигурации свойства Work1ID
            IProperty? id1Property = entityType.FindProperty(nameof(ComparisonReport.Work1ID));
            Assert.NotNull(id1Property);
            Assert.Equal("work1_id", id1Property.GetColumnName());

            // Проверка конфигурации свойства Work2ID
            IProperty? id2Property = entityType.FindProperty(nameof(ComparisonReport.Work2ID));
            Assert.NotNull(id2Property);
            Assert.Equal("work2_id", id2Property.GetColumnName());

            // Проверка конфигурации свойства Similarity
            IProperty? similarityProperty = entityType.FindProperty(nameof(ComparisonReport.Similarity));
            Assert.NotNull(similarityProperty);
            Assert.Equal("similarity", similarityProperty.GetColumnName());
        }
    }
}
