using System.Net;
using Sellow.Shared.Abstractions.Exceptions;

namespace Sellow.Modules.Sales.Application.Features.Categories.Exceptions;

internal sealed class CategoryNotFoundException : SellowException
{
    public override HttpStatusCode HttpCode => HttpStatusCode.NotFound;
    public override string ErrorCode => "category_not_found";

    public CategoryNotFoundException(Guid id) : base($"Category with an id: '{id}' was not found.")
    {
    }
}