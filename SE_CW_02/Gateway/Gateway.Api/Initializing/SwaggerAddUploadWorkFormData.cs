using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Gateway.Api.Initializing
{
    /// <summary>
    /// Добавление формы для загрузки работы.
    /// </summary>
    public class SwaggerAddUploadWorkFormData : IOperationFilter
    {
        /// <summary>
        /// Обработчик Swagger генератора.
        /// </summary>
        /// <param name="operation">Операция endpoint.</param>
        /// <param name="context">Контекст.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Проверка типа запроса.
            string? httpMethod = context.ApiDescription.HttpMethod?.ToUpperInvariant();
            if (httpMethod != "POST") return;

            // Проверка пути запроса.
            string? apiPath = context.ApiDescription.RelativePath?.ToLowerInvariant();
            if (apiPath != "api/works") return;

            // Устанавливаем multipart/form-data.
            operation.RequestBody = new OpenApiRequestBody
            {
                Content =
                {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties = new Dictionary<string, OpenApiSchema>
                            {
                                ["UserID"] = new OpenApiSchema
                                {
                                    Type = "integer",
                                    Format = "int32"
                                },
                                ["file"] = new OpenApiSchema
                                {
                                    Type = "string",
                                    Format = "binary"
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
