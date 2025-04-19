using Microsoft.Extensions.DependencyInjection;
using SE_HW_02.Entities.Events;
using SE_HW_02.Entities.Models;
using SE_HW_02.UseCases.Animals;
using SE_HW_02.UseCases.Enclosures;

namespace SE_HW_02.UseCases.AnimalEnclosure
{
    public class AnimalTransferService : IAnimalTransferService
    {
        public event EventHandler<AnimalMovedEvent>? OnAnimalTransfered;

        private IAnimalRepository animalRepository;
        private IEnclosureRepository enclosureRepository;

        public AnimalTransferService(IServiceProvider provider)
        {
            animalRepository = provider.GetRequiredService<IAnimalRepository>();
            enclosureRepository = provider.GetRequiredService<IEnclosureRepository>();
        }

        public bool Register(int animalID, int enclosureID)
        {
            // Получение животного.
            Animal? animal = animalRepository.Get(animalID);
            if (animal == null) return false;

            // Получение вольера.
            Enclosure? enclosure = enclosureRepository.Get(enclosureID);
            if (enclosure == null) return false;

            // Проверка свободного места в вольере.
            if (enclosure.AnimalsAmount >= enclosure.AnimalsCapacity) return false;

            // Заселение животного в вольер.
            ++enclosure.AnimalsAmount;
            animal.EnclosureID = enclosureID;

            // Сохранение информации.
            return animalRepository.Update(animalID, animal) && enclosureRepository.Update(enclosureID, enclosure);
        }

        public bool Unregister(int animalID)
        {
            // Получение животного.
            Animal? animal = animalRepository.Get(animalID);
            if (animal == null) return false;

            // Получение вольера.
            Enclosure? enclosure = enclosureRepository.Get(animal.EnclosureID);
            if (enclosure == null) return false;

            // Выселение животного из вольера.
            --enclosure.AnimalsAmount;
            animal.EnclosureID = -1;

            // Сохранение информации.
            return animalRepository.Update(animalID, animal) && enclosureRepository.Update(enclosure.ID, enclosure);
        }

        public bool Transfer(int animalID, int enclosureID)
        {
            // Получение животного.
            Animal? animal = animalRepository.Get(animalID);
            if (animal == null) return false;

            if (animal.EnclosureID == enclosureID) return true;

            // Получение вольера откуда животное переселяется.
            Enclosure? from = enclosureRepository.Get(animal.EnclosureID);
            if (from == null) return false;

            // Получение вольера куда животное переселяется.
            Enclosure? to = enclosureRepository.Get(enclosureID);
            if (to == null) return false;

            // Переселение животного.
            --from.AnimalsAmount;
            ++to.AnimalsAmount;
            animal.EnclosureID = enclosureID;

            // Сохранение информации.
            if (!enclosureRepository.Update(from.ID, from) ||
                !enclosureRepository.Update(to.ID, to) ||
                !animalRepository.Update(animalID, animal))
            {
                return false;
            }

            // Создание события.
            OnAnimalTransfered?.Invoke(this, new(animalID, from.ID, to.ID));
            return true;
        }
    }
}
