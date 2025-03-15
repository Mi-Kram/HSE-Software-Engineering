namespace HseBankLibrary.Storage.AutoIncrement
{
    /// <summary>
    /// Самоувеличивающийся ID для типа String.
    /// </summary>
    public class StringAutoIncrement : IAutoIncrement<string>
    {
        public string GetNext()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
