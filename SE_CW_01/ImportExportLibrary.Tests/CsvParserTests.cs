using ImportExportLibrary.Exceptions;
using Moq;
using YamlDotNet.Serialization;

namespace ImportExportLibrary.Tests
{
    public class CsvParserTests
    {
        [Fact]
        public void Export()
        {
            CsvParser<Person> parser = new();

            List<Person> lst =
            [
                new Person{ Name = "abc", Age = 5 },
                new Person{ Name = "xyz", Age = 7 }
            ];

            using StringWriter sw = new();
            parser.Export(lst, sw);

            string expected = @"Name,Age
abc,5
xyz,7
";

            Assert.Equal(expected, sw.ToString());
        }

        [Fact]
        public void Export_Exception()
        {
            CsvParser<Person> parser = new();

            List<Person> lst =
            [
                new Person{ Name = "abc", Age = 5 },
                new Person{ Name = "xyz", Age = 7 }
            ];

            StringWriter sw = new();
            sw.Dispose();

            Assert.Throws<ExportException>(() => parser.Export(lst, sw));
        }

        [Fact]
        public void Import()
        {
            CsvParser<Person> parser = new();

            string csv = @"Name,Age
abc,5
xyz,7
";

            using StringReader sr = new(csv);
            List<Person> lst = [.. parser.Import(sr)];

            Assert.Equal(2, lst.Count);
            Assert.Equal("abc", lst[0].Name);
            Assert.Equal("xyz", lst[1].Name);
            Assert.Equal(5, lst[0].Age);
            Assert.Equal(7, lst[1].Age);
        }

        [Fact]
        public void Import_Exception()
        {
            CsvParser<Person> parser = new();

            string csv = @"ImportException";
            TextReader reader = new StringReader(csv);
            reader.Dispose();

            Assert.Throws<ImportException>(() => parser.Import(reader));
        }
    }
}
