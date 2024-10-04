using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Sellow.Shared.Infrastructure.Api;

public static class ControllerExtensions
{
    public static string GetActionUrl(this HttpRequest request)
        => $"{request.Scheme}://{request.Host.Value}/{request.Path}";

    public static Guid GetAuthenticatedUserId(this ClaimsPrincipal principal)
        => Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
}