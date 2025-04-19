using Microsoft.Extensions.DependencyInjection;
using SE_HW_02.Entities.Models;
using SE_HW_02.UseCases.AnimalEnclosure;
using SE_HW_02.UseCases.Feeding;

namespace SE_HW_02.UseCases.Animals
{
    /// <summary>
    /// Сервис животных.
    /// </summary>
    public class AnimalService : IAnimalService
    {
        private IAnimalRepository animalRepository;
        private IFeedingScheduleRepository feedingRepository;
        private IAnimalTransferService animalTransferService;

        public AnimalService(IServiceProvider provider)
        {
            animalRepository = provider.GetRequiredService<IAnimalRepository>();
            feedingRepository = provider.GetRequiredService<IFeedingScheduleRepository>();
            animalTransferService = provider.GetRequiredService<IAnimalTransferService>();
        }

        public int? Add(Animal animal)
        {
            ArgumentNullException.ThrowIfNull(animal, nameof(animal));

            // Добавление животного.
            int? id = animalRepository.Add(animal);
            if (id == null) return null;

            // Добавление животного в вольер.
            if (animalTransferService.Register(id.Value, animal.EnclosureID)) return id;
            animalRepository.Remove(id.Value);

            return null;
        }

        public Animal? Get(int id)
        {
            return animalRepository.Get(id);
        }

        public IEnumerable<Animal> GetAll()
        {
            return animalRepository.GetAll();
        }

        public bool Remove(int id)
        {
            // Проверка наличия ссылок на объект.
            if (feedingRepository.GetAll().Any(x => x.AnimalID == id)) return false;

            // Убрать животное из вольера.
            if (!animalTransferService.Unregister(id)) return false;

            // Удаление животного.
            return animalRepository.Remove(id);
        }

        public bool Update(int id, Animal animal)
        {
            ArgumentNullException.ThrowIfNull(animal, nameof(animal));

            // Получение сущности животного для обновления.
            Animal? current = animalRepository.Get(id);
            if (current == null) return false;

            // Идентификаторы старого и нового вольеров.
            int from = current.EnclosureID, to = animal.EnclosureID;

            // Если идентификаторы не равны, переместить животное.
            if (animal.EnclosureID != current.EnclosureID)
            {
                if (!animalTransferService.Transfer(id, to)) return false;
            }

            // Обновить животное.
            if (animalRepository.Update(id, animal)) return true;

            // Если обновить не получилось, переместить животное обратно.
            animalTransferService.Transfer(id, from);
            return false;
        }
    }
}
