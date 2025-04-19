namespace SE_HW_02.Entities.Models
{
	/// <summary>
	/// Сущность вольера.
	/// </summary>
    public class Enclosure : Entity
    {
		private int id;
		private string type = "";
		private Dimensions size;
		private int animalsAmount;
		private int animalsCapacity;

		/// <summary>
		/// Идентификатор.
		/// </summary>
		public override int ID
		{
			get => id;
			set => id = value;
		}

		/// <summary>
		/// Тип.
		/// </summary>
		public string Type
		{
			get => type;
			set => type = value ?? throw new ArgumentNullException(nameof(Type));
		}

		/// <summary>
		/// Размерр.
		/// </summary>
		public Dimensions Size
		{
			get => size;
			set => size = value;
		}

		/// <summary>
		/// Текущее количество животных в вольере.
		/// </summary>
        public int AnimalsAmount
        {
            get => animalsAmount;
            set => animalsAmount = value >= 0 ? value : throw new ArgumentException($"{nameof(AnimalsAmount)} должен быть больше или равен 0", nameof(AnimalsAmount));
        }

		/// <summary>
		/// Максимальная вместимость вольера.
		/// </summary>
        public int AnimalsCapacity
        {
            get => animalsCapacity;
            set => animalsCapacity = value >= 0 ? value : throw new ArgumentException($"{nameof(animalsCapacity)} должен быть больше или равен 0", nameof(animalsCapacity));
        }

        public override Enclosure Clone()
        {
			return new Enclosure()
			{
				ID = ID,
				Type = Type,
				AnimalsAmount = AnimalsAmount,
				AnimalsCapacity = AnimalsCapacity,
				Size = Size
			};
        }
    }
}
