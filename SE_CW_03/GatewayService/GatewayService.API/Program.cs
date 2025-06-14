using GatewayService.API.Initializing;

namespace GatewayService.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplication app = await WebApp.CreateAsync(args);
            await app.RunAsync();
        }
    }
}
