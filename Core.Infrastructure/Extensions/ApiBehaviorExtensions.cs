using Core.Infrastructure.ApiBehaviors;
using Core.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Text.Json;

namespace Core.Infrastructure.Extensions;

public static class ApiBehaviorExtensions
{
    public static IServiceCollection ConfigureApiBehavior(this IServiceCollection services)
    {
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ApiHttpContextBehavior<,>));

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .SelectMany(x => x.Value!.Errors)
                    .Select(e =>
                    {
                        try
                        {
                            return JsonSerializer.Deserialize<ErrorDetails>(e.ErrorMessage);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex, "Model validation failed.");

                            return new ErrorDetails(
                                "VALIDATION_ERROR",
                                e.ErrorMessage);
                        }
                    });

                Log.Error($"Model validation failed: {errors}");

                return new BadRequestObjectResult(ApiResult.Failure(errors.FirstOrDefault()));
            };
        });

        return services;
    }
}
