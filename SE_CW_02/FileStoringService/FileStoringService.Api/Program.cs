using FileStoringService.Api.Initializing;

namespace FileStoringService.Api
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
