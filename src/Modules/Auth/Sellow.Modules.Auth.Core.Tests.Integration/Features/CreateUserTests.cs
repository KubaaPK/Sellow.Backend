using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sellow.Modules.Auth.Contracts.Events;
using Sellow.Modules.Auth.Core.Auth;
using Sellow.Modules.Auth.Core.DAL.Repositories;
using Sellow.Modules.Auth.Core.Domain;
using Sellow.Modules.Auth.Core.Features;

namespace Sellow.Modules.Auth.Core.Tests.Integration.Features;

public sealed class CreateUserTests : IDisposable
{
    private Task<Guid> Act(CreateUser command) => _handler.Handle(command, default);

    [Fact]
    internal async Task should_create_a_new_user()
    {
        await _testDatabase.Init();
        var command = new CreateUser("jankowalski22@wp.pl", "jankowalski22", "super-strong-password");

        await Act(command);

        Assert.Equal(1, _testDatabase.Context.Users.Count());
    }

    [Fact]
    internal async Task should_not_allow_to_create_duplicated_user()
    {
        await _testDatabase.Init();
        var command = new CreateUser("jankowalski22@wp.pl", "jankowalski22", "super-strong-password");
        await Act(command);

        await Assert.ThrowsAsync<UserAlreadyExistsException>(() => Act(command));
    }

    [Fact]
    internal async Task should_user_be_created_in_external_auth_system()
    {
        await _testDatabase.Init();
        var command = new CreateUser("jankowalski22@wp.pl", "jankowalski22", "super-strong-password");

        await Act(command);

        await _authService.Received(1).CreateUser(Arg.Any<ExternalAuthUser>());
    }

    [Fact]
    internal async Task should_user_be_deleted_from_database_if_user_creation_in_external_auth_system_fails()
    {
        await _testDatabase.Init();
        _authService.CreateUser(Arg.Any<ExternalAuthUser>()).ThrowsAsync(new Exception());
        var command = new CreateUser("jankowalski22@wp.pl", "jankowalski22", "super-strong-password");

        _ = await Record.ExceptionAsync(() => Act(command));

        Assert.Equal(0, _testDatabase.Context.Users.Count());
    }

    [Fact]
    internal async Task should_an_event_be_published_upon_successful_user_creation()
    {
        await _testDatabase.Init();
        var command = new CreateUser("jankowalski22@wp.pl", "jankowalski22", "super-strong-password");

        await Act(command);

        await _publisher.Received(1).Publish(Arg.Any<UserCreated>());
    }

    #region Arrange

    private readonly TestDatabase _testDatabase;
    private readonly CreateUserHandler _handler;
    private readonly IAuthService _authService;
    private readonly IPublisher _publisher;

    public CreateUserTests()
    {
        _testDatabase = new TestDatabase();
        IUserRepository userRepository = new UserRepository(_testDatabase.Context);
        _authService = Substitute.For<IAuthService>();
        _publisher = Substitute.For<IPublisher>();
        _handler = new CreateUserHandler(
            Substitute.For<ILogger<CreateUserHandler>>(),
            userRepository,
            _authService,
            _publisher
        );
    }

    #endregion

    public void Dispose() => _testDatabase.Dispose();
}