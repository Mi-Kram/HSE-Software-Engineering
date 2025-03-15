using HseBankLibrary.Models.Domain.DTO;

namespace HseBankLibrary.Models.Domain
{
    /// <summary>
    /// Категория.
    /// </summary>
    public class Category : IEntity<ulong, CategoryDTO>
    {
        private string name = "";

        public ulong ID { get; set; }

        public string Name
        {
            get => name;
            set => name = value ?? throw new ArgumentNullException(nameof(Name));
        }

        public CategoryDTO ToDTO()
        {
            return new CategoryDTO
            {
                ID = ID,
                Name = Name,
            };
        }
    }
}
