using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FileStoringService.Api.Initializing
{
    /// <summary>
    /// Обработчик Swagger генератора для добавления в заголовки поля token.
    /// </summary>
    public class SwaggerAddTokenHeader : IOperationFilter
    {
        /// <summary>
        /// Обработчик Swagger генератора.
        /// </summary>
        /// <param name="operation">Операция endpoint.</param>
        /// <param name="context">Контекст.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Создание коллекции, если её не существует.
            operation.Parameters ??= [];

            // Добавление заголовка.
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "token",
                In = ParameterLocation.Header,
                Description = "access token",
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                }
            });
        }
    }
}
