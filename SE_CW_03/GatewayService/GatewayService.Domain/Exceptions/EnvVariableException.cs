namespace GatewayService.Domain.Exceptions
{
    public class EnvVariableException(string envName) : Exception($"{envName}: переменная среды не найдена")
    {
        public string EnvName { get; } = envName ?? throw new ArgumentNullException(nameof(envName));
    }
}
