using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace CalidadAireApi.Services;

public class BearerSecuritySchemeTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
        {
            ["Bearer"] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header
            }
        };

        foreach (var ruta in document.Paths.Values)
        {
            foreach (var operacion in ruta.Operations!.Values)
            {
                operacion.Security ??= new List<OpenApiSecurityRequirement>();
                operacion.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
                });
            }
        }

        return Task.CompletedTask;
    }
}
