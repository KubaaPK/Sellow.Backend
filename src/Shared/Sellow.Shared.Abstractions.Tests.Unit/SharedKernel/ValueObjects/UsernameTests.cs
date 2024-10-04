using Sellow.Shared.Abstractions.SharedKernel.ValueObjects;

namespace Sellow.Shared.Abstractions.Tests.Unit.SharedKernel.ValueObjects;

public sealed class UsernameTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("              ")]
    [InlineData("ab")]
    [InlineData("      ab      ")]
    [InlineData("waaaaaaaaaaaaaaaaaaaaaay-to-long-username")]
    internal void should_not_allow_to_create_an_invalid_username_value_object(string username)
        => Assert.Throws<InvalidUsernameException>(() => new Username(username));


    [Theory]
    [InlineData("abc")]
    [InlineData("              abc         ")]
    [InlineData("username.2")]
    [InlineData("super cool username")]
    internal void should_create_a_valid_username_value_object(string username)
        => Assert.NotNull(new Username(username).Value);
}