using Microsoft.Extensions.DependencyInjection;

namespace Sellow.Shared.Infrastructure.Cache;

internal static class Extensions
{
    public static IServiceCollection AddMemoryCache(this IServiceCollection services)
        => MemoryCacheServiceCollectionExtensions.AddMemoryCache(services);
}