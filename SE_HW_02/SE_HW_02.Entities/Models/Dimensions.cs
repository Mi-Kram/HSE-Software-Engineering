namespace SE_HW_02.Entities.Models
{
    /// <summary>
    /// Размеры: ширина, длина, высота.
    /// </summary>
    public struct Dimensions
    {
        private float width;
        private float length;
        private float height;

        /// <summary>
        /// Ширина.
        /// </summary>
        public float Width
        {
            get => width;
            set => width = value >= 0 ? value : throw new ArgumentException("Ширина должна быть больше или равна 0", nameof(Width));
        }

        /// <summary>
        /// Длина.
        /// </summary>
        public float Length
        {
            get => length;
            set => length = value >= 0 ? value : throw new ArgumentException("Длина должна быть больше или равна 0", nameof(Length));
        }

        /// <summary>
        /// Высота.
        /// </summary>
        public float Height
        {
            get => height;
            set => height = value >= 0 ? value : throw new ArgumentException("Высота должна быть больше или равна 0", nameof(Height));
        }
    }
}
