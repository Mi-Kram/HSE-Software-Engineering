using Microsoft.Extensions.DependencyInjection;
using SE_HW_02.Entities.Models;

namespace SE_HW_02.UseCases.Enclosures
{
    public class EnclosureService : IEnclosureService
    {
        private IEnclosureRepository enclosureRepository;

        public EnclosureService(IServiceProvider provider)
        {
            enclosureRepository = provider.GetRequiredService<IEnclosureRepository>();
        }

        public int? Add(Enclosure enclosure)
        {
            ArgumentNullException.ThrowIfNull(enclosure, nameof(enclosure));

            // Проверка корректности входных данных.
            if (enclosure.AnimalsCapacity == 0 || enclosure.AnimalsAmount > enclosure.AnimalsCapacity) return null;
            if (enclosure.Size.Width <= 0 || enclosure.Size.Length <= 0 || enclosure.Size.Height <= 0) return null;
            
            // Добавление объекта.
            return enclosureRepository.Add(enclosure);
        }

        public Enclosure? Get(int id)
        {
            return enclosureRepository.Get(id);
        }

        public IEnumerable<Enclosure> GetAll()
        {
            return enclosureRepository.GetAll();
        }

        public bool Remove(int id)
        {
            Enclosure? enclosure = enclosureRepository.Get(id);
            
            // Проверка существование объекта и отсутствия ссылок на него.
            if (enclosure == null || enclosure.AnimalsAmount != 0) return false;

            // Удаление объекта.
            return enclosureRepository.Remove(id);
        }

        public bool Update(int id, Enclosure enclosure)
        {
            ArgumentNullException.ThrowIfNull(enclosure, nameof(enclosure));

            // Проверка корректности входных данных.
            if (enclosure.AnimalsCapacity == 0 || enclosure.AnimalsAmount > enclosure.AnimalsCapacity) return false;
            if (enclosure.Size.Width <= 0 || enclosure.Size.Length <= 0 || enclosure.Size.Height <= 0) return false;

            // Обновление объекта.
            return enclosureRepository.Update(id, enclosure);
        }
    }
}
