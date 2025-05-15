using Gateway.Api.Initializing;

namespace Gateway.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplication app = await WebApp.CreateAsync(args);
            app.Run();
        }
    }
}
