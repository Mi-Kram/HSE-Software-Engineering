namespace SE_HW_02.Infrastructure.Repositories
{
    /// <summary>
    /// Счётчик идентификатора.
    /// </summary>
    public class IdCounter
    {
        private int id = 0;

        public int Next()
        {
            return ++id;
        }
    }
}
