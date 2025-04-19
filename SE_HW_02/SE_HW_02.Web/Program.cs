using SE_HW_02.Infrastructure;
using SE_HW_02.UseCases;

namespace SE_HW_02.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplication app = InitApp(args);
            app.Run();
        }

        public static WebApplication InitApp(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // ���������� ��������..
            builder.Services.AddInfrastructure();
            builder.Services.AddUseCases();

            builder.Services.AddControllers();

            // �������� ������������ Swagger/OpenAPI �� ������ https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            WebApplication app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // ������������ ������ Http ��������.
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            return app;
        }
    }
}
