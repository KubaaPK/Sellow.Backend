using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Sellow.Modules.Sales.Application.Services;

namespace Sellow.Modules.Sales.Application;

internal static class Extensions
{
    internal static IServiceCollection AddApplication(this IServiceCollection services)
        => services
            .AddMediatR(options => options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true)
            .AddFluentValidationAutoValidation(options => options.DisableDataAnnotationsValidation = true)
            .AddScoped<ICategoryCacheService, CategoryCacheService>();
}