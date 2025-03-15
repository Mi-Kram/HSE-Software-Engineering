using HseBankLibrary.Models.Domain.DTO;
using HseBankLibrary.Models.Domain;

namespace HseBankLibrary.Factories
{
    /// <summary>
    /// Фабрика для создания категории.
    /// </summary>
    public class CategoryFactory : IDomainFactory<Category, CategoryDTO>
    {
        public Category Create(CategoryDTO args)
        {
            ArgumentNullException.ThrowIfNull(args);

            return new Category()
            {
                ID = args.ID,
                Name = args.Name ?? string.Empty,
            };
        }
    }
}
