using Core.Idempotencia;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Core.Infrastructure.Idempotencia;

public class AddIdempotencyKeyHeaderOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Verifica se o método da ação possui o atributo [SkipIdempotency]
        var skipIdempotency = context.MethodInfo.GetCustomAttribute<SkipIdempotencyAttribute>() != null;

        if (skipIdempotency)
            return; // Não adiciona o cabeçalho para esta operação
        

        // Verifica o método HTTP.
        // Podemos obter o HttpMethodAttribute (HttpGetAttribute, HttpPostAttribute, etc.)
        // e verificar qual é o verbo HTTP.
        var httpMethodAttribute = context.MethodInfo.GetCustomAttribute<HttpMethodAttribute>();
        if (httpMethodAttribute != null && httpMethodAttribute.HttpMethods.Contains("GET"))
            return; // Não adiciona o cabeçalho para operações GET

        // Garante que a lista de parâmetros não é nula
        operation.Parameters ??= new List<OpenApiParameter>();

        // Adiciona o parâmetro do cabeçalho Idempotency-Key
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = IdempotenciaConsts.HeaderKeyName,
            In = ParameterLocation.Header,
            Description = "Chave de idempotência para garantir que a operação seja executada apenas uma vez.",
            Required = true,
            Schema = new OpenApiSchema
            {
                Type = "string",
                // Sugere um novo GUID como valor padrão para facilitar o teste
                Default = new OpenApiString(Guid.NewGuid().ToString())
            }
        });
    }
}
