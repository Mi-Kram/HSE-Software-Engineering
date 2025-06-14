using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GatewayService.API.Initializing.Swagger
{
    public class CreateAccountFormData : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            string? httpMethod = context.ApiDescription.HttpMethod?.ToUpperInvariant();
            if (httpMethod != "POST") return;

            string? apiPath = context.ApiDescription.RelativePath?.ToLowerInvariant();
            if (apiPath != "api/accounts") return;

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
                                ["Caption"] = new OpenApiSchema
                                {
                                    Type = "string",
                                    Description = "Название счета",
                                    Default = new OpenApiString(""),
                                },
                                ["InitialBalance"] = new OpenApiSchema
                                {
                                    Type = "number",
                                    Description = "Начальный баланс"
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
