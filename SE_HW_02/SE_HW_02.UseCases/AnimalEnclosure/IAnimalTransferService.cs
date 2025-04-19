using SE_HW_02.Entities.Events;

namespace SE_HW_02.UseCases.AnimalEnclosure
{
    /// <summary>
    /// Сервис перемещения 
    /// </summary>
    public interface IAnimalTransferService
    {
        event EventHandler<AnimalMovedEvent>? OnAnimalTransfered;

        bool Register(int animalID, int enclosureID);
        bool Unregister(int animalID);
        bool Transfer(int animalID, int enclosureID);
    }
}
