namespace SE_HW_02.Entities.Models
{
    /// <summary>
    /// Сущность расписания кормления.
    /// </summary>
    public class FeedingSchedule : Entity
    {
		private int id;
		private int animalID;
		private TimeOnly time;
		private string food = string.Empty;

		/// <summary>
		/// Идентификатор.
		/// </summary>
		public override int ID
		{
			get => id;
			set => id = value;
		}

		/// <summary>
		/// Идентификатор животного.
		/// </summary>
		public int AnimalID
        {
			get => animalID;
			set => animalID = value;
		}

		/// <summary>
		/// Время кормления.
		/// </summary>
		public TimeOnly Time
		{
			get => time;
			set => time = value;
		}

		/// <summary>
		/// Еда.
		/// </summary>
		public string Food
        {
			get => food;
			set => food = value ?? throw new ArgumentNullException(nameof(Food));
		}

        public override FeedingSchedule Clone()
        {
			return new FeedingSchedule()
			{
				ID = ID,
				Food = Food,
				AnimalID = AnimalID,
				Time = Time
			};
        }
    }
}
