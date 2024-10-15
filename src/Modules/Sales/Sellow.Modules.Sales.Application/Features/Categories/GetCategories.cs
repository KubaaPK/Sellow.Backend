using MediatR;
using Microsoft.EntityFrameworkCore;
using Sellow.Modules.Sales.Infrastructure.DAL;

namespace Sellow.Modules.Sales.Application.Features.Categories;

internal sealed class GetCategories : IRequest<IEnumerable<CategoryDto>>;

internal sealed class GetCategoriesHandler : IRequestHandler<GetCategories, IEnumerable<CategoryDto>>
{
    private readonly SalesDbContext _context;

    public GetCategoriesHandler(SalesDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CategoryDto>> Handle(GetCategories request, CancellationToken cancellationToken)
    {
        var categories = await _context.Categories.ToListAsync(cancellationToken);
        return CategoryDto.BuildCategoryTree(categories);
    }
}