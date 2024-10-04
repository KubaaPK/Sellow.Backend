using System.Net;
using System.Text.RegularExpressions;
using Sellow.Shared.Abstractions.Exceptions;

namespace Sellow.Shared.Abstractions.SharedKernel.ValueObjects;

public sealed record Email
{
    private static readonly Regex EmailRegex = new(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidEmailException(value);
        }

        value = value.ToLowerInvariant().Trim();

        if (EmailRegex.IsMatch(value) is false)
        {
            throw new InvalidEmailException(value);
        }

        Value = value;
    }

    public static implicit operator Email(string email) => new(email);

    public static implicit operator string(Email email) => email.Value;
}

public sealed class InvalidEmailException : SellowException
{
    public override HttpStatusCode HttpCode => HttpStatusCode.BadRequest;
    public override string ErrorCode => "invalid_email";

    public InvalidEmailException(string email) : base($"Email '{email}' is invalid.")
    {
    }
}