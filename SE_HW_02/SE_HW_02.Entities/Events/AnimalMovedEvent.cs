namespace SE_HW_02.Entities.Events
{
    /// <summary>
    /// Информация о событии перемещения животного.
    /// </summary>
    /// <param name="AnimalID">id животного</param>
    /// <param name="FromEnclosureID">id вольера, с которого животное перемещается.</param>
    /// <param name="ToEnclosureID">id вольера, в который животное перемещается.</param>
    public record AnimalMovedEvent(int AnimalID, int FromEnclosureID, int ToEnclosureID);
}
