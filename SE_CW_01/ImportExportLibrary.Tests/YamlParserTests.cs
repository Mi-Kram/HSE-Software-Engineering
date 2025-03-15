using ImportExportLibrary.Exceptions;

namespace ImportExportLibrary.Tests
{
    public class YamlParserTests
    {
        [Fact]
        public void Export()
        {
            YamlParser<Person> parser = new();

            List<Person> lst =
            [
                new Person{ Name = "abc", Age = 5 },
                new Person{ Name = "xyz", Age = 7 }
            ];

            using StringWriter sw = new();
            parser.Export(lst, sw);

            string expected = @"- Name: abc
  Age: 5
- Name: xyz
  Age: 7
";

            Assert.Equal(expected, sw.ToString());
        }

        [Fact]
        public void Export_Exception()
        {
            YamlParser<Person> parser = new();

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
            YamlParser<Person> parser = new();

            string yaml = @"- Name: abc
  Age: 5
- Name: xyz
  Age: 7
";

            using StringReader sr = new(yaml);
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
            YamlParser<Person> parser = new();

            string yaml = "ImportException";

            using StringReader sr = new(yaml);
            Assert.Throws<ImportException>(() => parser.Import(sr));
        }
    }
}
