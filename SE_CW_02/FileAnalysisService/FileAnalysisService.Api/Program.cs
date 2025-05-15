using FileAnalysisService.Api.Initializing;

namespace FileAnalysisService.Api
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
