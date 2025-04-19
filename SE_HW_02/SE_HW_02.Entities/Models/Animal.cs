namespace SE_HW_02.Entities.Models
{
	/// <summary>
	/// Сущность животного.
	/// </summary>
    public class Animal : Entity
    {
		private int id;
		private string name = string.Empty;
		private string type = string.Empty;
		private DateTime birthday;
		private bool isMale;
		private bool isHealthy;
		private string favoriteFood = string.Empty;
		private int enclosureID;

        /// <summary>
        /// Идентификатор.
        /// </summary>
        public override int ID
		{
			get => id;
			set => id = value;
		}

		/// <summary>
		/// Кличка.
		/// </summary>
		public string Name
		{
			get => name;
			set => name = value ?? throw new ArgumentNullException(nameof(Name));
		}

		/// <summary>
		/// Вид.
		/// </summary>
		public string Type
		{
			get => type;
			set => type = value ?? throw new ArgumentNullException(nameof(Type));
		}

		/// <summary>
		/// День рождения.
		/// </summary>
		public DateTime Birthday
        {
			get => birthday;
			set => birthday = value;
		}

		/// <summary>
		/// Флаг озачающий мужской пол.
		/// </summary>
		public bool IsMale
		{
			get => isMale;
			set => isMale = value;
		}

		/// <summary>
		/// Флаг означающий здоровье животного.
		/// </summary>
		public bool IsHealthy
		{
			get => isHealthy;
			set => isHealthy = value;
		}

		/// <summary>
		/// Любимая еда.
		/// </summary>
		public string FavoriteFood
        {
			get => favoriteFood;
			set => favoriteFood = value ?? throw new ArgumentNullException(nameof(FavoriteFood));
		}

		/// <summary>
		/// Идентификатор вольера.
		/// </summary>
		public int EnclosureID
        {
			get => enclosureID;
			set => enclosureID = value;
		}

        public override Animal Clone()
        {
			return new Animal()
			{
				ID = ID,
				Name = Name,
				FavoriteFood = FavoriteFood,
				Type = Type,
				IsHealthy = IsHealthy,
				IsMale = IsMale,
				Birthday = Birthday,
				EnclosureID = EnclosureID
			};
        }
    }
}
