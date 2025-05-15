namespace FileAnalysisService.Domain.Models
{
    /// <summary>
    /// id работ сравнения.
    /// </summary>
    public class ComparisonKey(int work1ID, int work2ID)
    {
        /// <summary>
        /// id первой работы.
        /// </summary>
        public int Work1ID { get; } = Math.Min(work1ID, work2ID);

        /// <summary>
        /// id второй работы.
        /// </summary>
        public int Work2ID { get; } = Math.Max(work1ID, work2ID);

        public override int GetHashCode()
        {
            return HashCode.Combine(Work1ID, Work2ID);
        }

        public static bool operator==(ComparisonKey a, ComparisonKey b)
        {
            return a.Work1ID == b.Work1ID && a.Work2ID == b.Work2ID;
        }

        public static bool operator!=(ComparisonKey a, ComparisonKey b)
        {
            return !(a == b);
        }

        public override bool Equals(object? obj)
        {
            return obj is ComparisonKey key && this == key;
        }
    }
}
