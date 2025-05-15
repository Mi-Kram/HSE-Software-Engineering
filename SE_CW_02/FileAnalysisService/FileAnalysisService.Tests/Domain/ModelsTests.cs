using FileAnalysisService.Domain.Entities;
using FileAnalysisService.Domain.Exceptions;
using FileAnalysisService.Domain.Models;

namespace FileAnalysisService.Tests.Domain
{
    public class ModelsTests
    {
        [Fact]
        public void ApplicationVariables_Tests()
        {
            Assert.NotNull(ApplicationVariables.TOKEN);
            Assert.NotNull(ApplicationVariables.DB_CONNECTION);
            Assert.NotNull(ApplicationVariables.MAIN_SERVER);
            Assert.NotNull(ApplicationVariables.COMPARISON_FILE);
            Assert.NotNull(ApplicationVariables.WORDS_CLOUD_SRORAGE_ADDRESS);
            Assert.NotNull(ApplicationVariables.WORDS_CLOUD_SRORAGE_LOGIN);
            Assert.NotNull(ApplicationVariables.WORDS_CLOUD_SRORAGE_PASSWORD);
            Assert.NotNull(ApplicationVariables.WORDS_CLOUD_SRORAGE_BUCKET);
            Assert.NotNull(ApplicationVariables.WORDS_CLOUD_SRORAGE_SSL);
        }

        [Fact]
        public void ComparisonKey_Tests()
        {
            ComparisonKey key1 = new(5, 3);
            ComparisonKey key2 = new(4, 8);

            Assert.Equal(3, key1.Work1ID);
            Assert.Equal(5, key1.Work2ID);
            Assert.Equal(4, key2.Work1ID);
            Assert.Equal(8, key2.Work2ID);

            Assert.Equal(HashCode.Combine(3, 5), key1.GetHashCode());
            Assert.True(key1 == new ComparisonKey(3, 5));
            Assert.True(key1 == new ComparisonKey(5, 3));
            Assert.True(key1 != key2);

            Assert.True(key1.Equals(key1));
            Assert.False(key1.Equals(key2));
        }

        [Fact]
        public void EnvVariableException_Tests()
        {
            EnvVariableException ex = new("myEnv");
            Assert.Equal("myEnv", ex.EnvName);
        }

        [Fact]
        public void AnalyzeReport_Tests()
        {
            AnalyzeReport report = new()
            {
                WorkID = 5,
                Paragraphs = 7,
                Words = 23,
                Numbers = 3,
                Symbols = 321
            };

            Assert.Equal(5, report.WorkID);
            Assert.Equal(7, report.Paragraphs);
            Assert.Equal(23, report.Words);
            Assert.Equal(3, report.Numbers);
            Assert.Equal(321, report.Symbols);

            Assert.Throws<ArgumentException>(() => report.Paragraphs = -1);
            Assert.Throws<ArgumentException>(() => report.Words = -1);
            Assert.Throws<ArgumentException>(() => report.Numbers = -1);
            Assert.Throws<ArgumentException>(() => report.Symbols = -1);
        }

        [Fact]
        public void ComparisonReport_Tests()
        {
            ComparisonReport report = new()
            {
                Work1ID = 5,
                Work2ID = 7,
                Similarity = 0.64f,
            };

            Assert.Equal(5, report.Work1ID);
            Assert.Equal(7, report.Work2ID);
            Assert.Equal(0.64f, report.Similarity);

            Assert.Throws<ArgumentException>(() => report.Similarity = -1);
            Assert.Throws<ArgumentException>(() => report.Similarity = 1.5f);
        }
    }
}
