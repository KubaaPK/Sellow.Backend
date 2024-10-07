using System.Net;
using Sellow.Shared.Abstractions.Exceptions;

namespace Sellow.Shared.Abstractions.SharedKernel.ValueObjects;

public sealed record Username
{
    public const int MinimumUsernameLength = 3;
    public const int MaximumUsernameLength = 25;

    public string Value { get; }

    public Username(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidUsernameException(value);
        }

        value = value.Trim();

        if (value.Length is < MinimumUsernameLength or > MaximumUsernameLength)
        {
            throw new InvalidUsernameException(value);
        }

        Value = value;
    }

    public static implicit operator Username(string username) => new(username);

    public static implicit operator string(Username username) => username.Value;
}

public sealed class InvalidUsernameException : SellowException
{
    public override HttpStatusCode HttpCode => HttpStatusCode.BadRequest;
    public override string ErrorCode => "invalid_username";

    public InvalidUsernameException(string username) : base($"Username '{username}' is invalid.")
    {
    }
}