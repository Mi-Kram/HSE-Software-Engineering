namespace FileStoringService.Domain.Exceptions
{
    /// <summary>
    /// Исключение при отсутствии переменной среды с именем <see cref="EnvName"/>.
    /// </summary>
    /// <param name="envName"></param>
    public class EnvVariableException(string envName) : Exception($"{envName}: переменная среды не найдена")
    {
        /// <summary>
        /// Имя переменной среды.
        /// </summary>
        public string EnvName { get; } = envName ?? throw new ArgumentNullException(nameof(envName));
    }
}
