using MediatR;

namespace Sellow.Modules.Auth.Contracts.Events;

public sealed record UserCreated(Guid UserId, string Email, string Username) : INotification;