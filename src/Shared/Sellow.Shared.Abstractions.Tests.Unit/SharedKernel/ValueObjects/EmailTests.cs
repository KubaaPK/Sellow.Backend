using Sellow.Shared.Abstractions.SharedKernel.ValueObjects;

namespace Sellow.Shared.Abstractions.Tests.Unit.SharedKernel.ValueObjects;

public sealed class EmailTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("        ")]
    [InlineData("@")]
    [InlineData("invalid@")]
    [InlineData("invalid@email")]
    [InlineData("invalid@email.")]
    [InlineData("@email.com")]
    internal void should_not_allow_to_create_an_invalid_email_value_object(string email)
        => Assert.Throws<InvalidEmailException>(() => new Email(email));

    [Theory]
    [InlineData("valid@email.com")]
    [InlineData("val.id@email.com")]
    internal void should_create_a_valid_email_value_object(string email)
        => Assert.Equal(email, new Email(email).Value);
}