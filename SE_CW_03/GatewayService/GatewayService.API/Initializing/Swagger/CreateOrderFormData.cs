using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GatewayService.API.Initializing.Swagger
{
    public class CreateOrderFormData : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            string? httpMethod = context.ApiDescription.HttpMethod?.ToUpperInvariant();
            if (httpMethod != "POST") return;

            string? apiPath = context.ApiDescription.RelativePath?.ToLowerInvariant();
            if (apiPath != "api/orders") return;

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
                                    Description = "ID счета"
                                },
                                ["Bill"] = new OpenApiSchema
                                {
                                    Type = "number",
                                    Description = "Стоимость заказа"
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
