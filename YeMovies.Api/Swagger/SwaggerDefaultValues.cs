using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace YeMoviesApi.Swagger;

public class SwaggerDefaultValues : IOperationFilter
{
    public void Apply(
        OpenApiOperation operation,
        OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;
        
        foreach (var responseType in apiDescription.SupportedResponseTypes)
        {
            var responseKey = responseType.IsDefaultResponse
                ? "default"
                : responseType.StatusCode.ToString();

            if (operation.Responses is null ||
                !operation.Responses.TryGetValue(responseKey, out var response))
            {
                continue;
            }

            if (response.Content is null)
            {
                continue;
            }

            foreach (var contentType in response.Content.Keys.ToArray())
            {
                if (responseType.ApiResponseFormats.All(
                        x => x.MediaType != contentType))
                {
                    response.Content.Remove(contentType);
                }
            }
        }

        if (operation.Parameters is null)
        {
            return;
        }

        foreach (var parameter in operation.Parameters)
        {
            if (parameter is not OpenApiParameter openApiParameter)
            {
                continue;
            }

            var description = apiDescription.ParameterDescriptions
                .FirstOrDefault(p => p.Name == parameter.Name);

            if (description is null)
            {
                continue;
            }

            openApiParameter.Description ??=
                description.ModelMetadata?.Description;

            if (openApiParameter.Schema is OpenApiSchema schema &&
                schema.Default is null &&
                description.DefaultValue is not null)
            {
                schema.Default = JsonSerializer.SerializeToNode(
                    description.DefaultValue,
                    description.ModelMetadata?.ModelType
                        ?? description.DefaultValue.GetType());
            }

            openApiParameter.Required |= description.IsRequired;
        }
    }
}