using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sellow.Shared.Infrastructure.Options;

public static class Extensions
{
    public static T GetOptions<T>(this IServiceCollection services, string section) where T : new()
    {
        using var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var options = new T();
        configuration.GetSection(section).Bind(options);

        return options;
    }
}