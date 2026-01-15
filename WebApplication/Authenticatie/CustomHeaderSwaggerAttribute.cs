using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApplication1.Authenticatie
{
    public class CustomHeaderSwaggerAttribute : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var globalAttributes = context.ApiDescription.ActionDescriptor.FilterDescriptors.Select(p => p.Filter);
            var controllerAttributes = context.MethodInfo?.DeclaringType?.GetCustomAttributes(true);
            var methodAttributes = context.MethodInfo?.GetCustomAttributes(true);
            var produceAttributes = globalAttributes
                .Union(controllerAttributes ?? throw new InvalidOperationException())
                .Union(methodAttributes)
                .OfType<AuthorizeAttribute>()
                .ToList();

            if (produceAttributes.Count == 0) // De controller of methode vereist geen authenticatie (API-Key based)
                return;

            if (operation.Parameters == null)
                operation.Parameters = new List<IOpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-API-Key", //Add Key Name
                In = ParameterLocation.Header, //Set Location
                Required = true,
                Description = "Add description if necessary",
                Schema = new OpenApiSchema
                {
                    Type = JsonSchemaType.String
                }
            });
        }
    }
}
