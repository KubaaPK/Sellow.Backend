using System.Net;

namespace Sellow.Shared.Abstractions.Exceptions;

public abstract class SellowException : Exception
{
    public abstract HttpStatusCode HttpCode { get; }
    public abstract string ErrorCode { get; }

    protected SellowException(string message) : base(message)
    {
    }
}