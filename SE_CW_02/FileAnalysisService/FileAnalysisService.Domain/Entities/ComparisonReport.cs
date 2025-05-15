namespace FileAnalysisService.Domain.Entities
{
    /// <summary>
    /// Отчёт стравнения 2 работ.
    /// </summary>
    public class ComparisonReport
    {
        private int work1ID;
        private int work2ID;
        private float similarity;

        /// <summary>
        /// id первой работы.
        /// </summary>
        public int Work1ID
        {
            get => work1ID;
            set => work1ID = value;
        }

        /// <summary>
        /// id второй работы.
        /// </summary>
        public int Work2ID
        {
            get => work2ID;
            set => work2ID = value;
        }

        /// <summary>
        /// Процент схожести.
        /// </summary>
        public float Similarity
        {
            get => similarity;
            set => similarity = value is >= 0 and <= 1 ? value : throw new ArgumentException("Процент схожести должен быть в диапазоне от 0 до 1");
        }
    }
}
