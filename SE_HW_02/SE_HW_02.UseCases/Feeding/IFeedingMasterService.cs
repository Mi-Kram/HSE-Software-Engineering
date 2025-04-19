namespace SE_HW_02.UseCases.Feeding
{
    /// <summary>
    /// Сервис кормления животного.
    /// </summary>
    public interface IFeedingMasterService
    {
        /// <summary>
        /// Покормить животное согласно расписанию.
        /// </summary>
        /// <param name="feedingScheduleID">Идентификатор расписания.</param>
        /// <returns>True, если животное покормлено, иначе - False.</returns>
        public bool Feed(int feedingScheduleID);

        /// <summary>
        /// Покормить животное.
        /// </summary>
        /// <param name="animalID">Идентификатор животного.</param>
        /// <param name="food">Еда для кормления.</param>
        /// <returns>True, если животное покормлено, иначе - False.</returns>
        public bool Feed(int animalID, string food);
    }
}
