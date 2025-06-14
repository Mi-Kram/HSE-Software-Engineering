using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GatewayService.API.Initializing.Swagger
{
    public class TopUpBalanceFormData : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            string? httpMethod = context.ApiDescription.HttpMethod?.ToUpperInvariant();
            if (httpMethod != "PATCH") return;

            string? apiPath = context.ApiDescription.RelativePath?.ToLowerInvariant();
            if (apiPath != "api/accounts/top-up") return;

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
                                ["Operation"] = new OpenApiSchema
                                {
                                    Type = "number",
                                    Description = "Сумма поплнения"
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
