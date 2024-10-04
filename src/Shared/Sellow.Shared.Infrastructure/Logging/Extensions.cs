using Microsoft.AspNetCore.Builder;
using Serilog;

namespace Sellow.Shared.Infrastructure.Logging;

internal static class Extensions
{
    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((_, options) => options.WriteTo.Console());

        return builder;
    }
}