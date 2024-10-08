using MediatR;
using Sellow.Modules.Auth.Core.Auth;

namespace Sellow.Modules.Auth.Core.Features;

internal sealed record ActivateUser(Guid Id) : IRequest;

internal sealed class ActivateUserHandler : IRequestHandler<ActivateUser>
{
    private readonly IAuthService _authService;

    public ActivateUserHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task Handle(ActivateUser request, CancellationToken cancellationToken)
    {
        await _authService.ActivateUser(request.Id, cancellationToken);
    }
}