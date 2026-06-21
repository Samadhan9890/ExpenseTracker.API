using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace ExpenseTracker.Services.Middleware
{

    public class FileUploadOperation : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Retrieve all parameters from the method
            var parameters = context.MethodInfo.GetParameters();

            // Only handle multipart/form-data
            if (parameters.Any(p => p.GetCustomAttributes(typeof(Microsoft.AspNetCore.Mvc.FromFormAttribute), false).Any()))
            {
                var formDataParameters = parameters
                    .Where(p => p.GetCustomAttributes(typeof(Microsoft.AspNetCore.Mvc.FromFormAttribute), false).Any())
                    .Select(p => new KeyValuePair<string, OpenApiSchema>(p.Name, new OpenApiSchema
                    {
                        Type = "string",
                        Format = p.ParameterType == typeof(IFormFile) ? "binary" : null
                    }))
                    .ToDictionary(x => x.Key, x => x.Value);

                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["multipart/form-data"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                Properties = formDataParameters
                            }
                        }
                    }
                };
            }
        }
    }
}
