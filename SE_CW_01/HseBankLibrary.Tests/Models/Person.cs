using HseBankLibrary.Models.Domain;

namespace HseBankLibrary.Tests.Models
{
    public class Person : IEntity<ulong, Person>
    {
        public ulong ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }

        public Person ToDTO() => this;
    }
}
