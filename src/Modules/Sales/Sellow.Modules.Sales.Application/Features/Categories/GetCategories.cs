using MediatR;
using Sellow.Modules.Sales.Application.Services;

namespace Sellow.Modules.Sales.Application.Features.Categories;

internal sealed class GetCategories : IRequest<IEnumerable<CategoryDto>>;

internal sealed class GetCategoriesHandler : IRequestHandler<GetCategories, IEnumerable<CategoryDto>>
{
    private readonly ICategoryCacheService _categoryCacheService;

    public GetCategoriesHandler(ICategoryCacheService categoryCacheService)
    {
        _categoryCacheService = categoryCacheService;
    }

    public async Task<IEnumerable<CategoryDto>> Handle(GetCategories request, CancellationToken cancellationToken)
        => await _categoryCacheService.GetCategories(cancellationToken);
}