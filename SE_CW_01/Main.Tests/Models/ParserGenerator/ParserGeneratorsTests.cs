using ImportExportLibrary;
using Main.Models.ParserGenerator;
using System.Text;

namespace Main.Tests.Models.ParserGenerator
{
    public class ParserGeneratorsTests
    {
        [Fact]
        public void CsvParserGeneratorTests()
        {
            CsvParserGenerator generator = new();
            var parser = generator.Create<StringBuilder>();
            Assert.IsType<CsvParser<StringBuilder>>(parser);
        }

        [Fact]
        public void JsonParserGeneratorTests()
        {
            JsonParserGenerator generator = new();
            var parser = generator.Create<StringBuilder>();
            Assert.IsType<JsonParser<StringBuilder>>(parser);
        }

        [Fact]
        public void YamlParserGeneratorTests()
        {
            YamlParserGenerator generator = new();
            var parser = generator.Create<StringBuilder>();
            Assert.IsType<YamlParser<StringBuilder>>(parser);
        }
    }
}
