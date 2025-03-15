namespace HseBankLibrary.Storage.AutoIncrement
{
    /// <summary>
    /// Самоувеличивающийся ID для типа Ulong.
    /// </summary>
    public class UlongAutoIncrement : IAutoIncrement<ulong>
    {
        private ulong nextID = 0;

        public UlongAutoIncrement() { }

        public ulong GetNext()
        {
            return nextID++;
        }
    }
}
