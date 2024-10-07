using System.Net;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Sellow.Modules.Auth.Contracts.Events;
using Sellow.Modules.Auth.Core.Auth;
using Sellow.Modules.Auth.Core.Domain;
using Sellow.Shared.Abstractions.Exceptions;
using Sellow.Shared.Abstractions.SharedKernel.ValueObjects;

namespace Sellow.Modules.Auth.Core.Features;

internal sealed record CreateUser(string Email, string Username, string Password) : IRequest<Guid>;

internal sealed class CreateUserValidator : AbstractValidator<CreateUser>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail address is required.")
            .EmailAddress().WithMessage("Invalid e-mail address.");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(Username.MinimumUsernameLength).WithMessage(
                $"Username must be {Username.MinimumUsernameLength} - {Username.MaximumUsernameLength} characters long.")
            .MaximumLength(Username.MaximumUsernameLength).WithMessage(
                $"Username must be {Username.MinimumUsernameLength} - {Username.MaximumUsernameLength} characters long.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
    }
}

internal sealed class CreateUserHandler : IRequestHandler<CreateUser, Guid>
{
    private readonly ILogger<CreateUserHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;
    private readonly IPublisher _publisher;

    public CreateUserHandler(ILogger<CreateUserHandler> logger, IUserRepository userRepository,
        IAuthService authService, IPublisher publisher)
    {
        _logger = logger;
        _userRepository = userRepository;
        _authService = authService;
        _publisher = publisher;
    }

    public async Task<Guid> Handle(CreateUser request, CancellationToken cancellationToken)
    {
        var user = new User(request.Email, request.Username);

        var isNewUserUnique = await _userRepository.IsUserUnique(user, cancellationToken);
        if (isNewUserUnique is false)
        {
            throw new UserAlreadyExistsException();
        }

        await _userRepository.Save(user, cancellationToken);

        try
        {
            var externalAuthUser = new ExternalAuthUser(user.Id, user.Email, user.Username, request.Password);
            await _authService.CreateUser(externalAuthUser, cancellationToken);
        }
        catch
        {
            await _userRepository.Delete(user, cancellationToken);
            throw;
        }

        var userCreated = new UserCreated(user.Id, user.Email, user.Username);
        await _publisher.Publish(userCreated, cancellationToken);

        _logger.LogInformation("User {Id} has been created", user.Id);

        return user.Id;
    }
}

internal sealed class UserAlreadyExistsException : SellowException
{
    public override HttpStatusCode HttpCode => HttpStatusCode.Conflict;
    public override string ErrorCode => "user_already_exists";

    public UserAlreadyExistsException() : base("User with given credentials already exists.")
    {
    }
}