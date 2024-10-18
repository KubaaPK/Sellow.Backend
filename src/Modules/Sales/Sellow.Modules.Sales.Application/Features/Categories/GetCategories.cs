using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Sellow.Modules.Sales.Infrastructure.DAL;

namespace Sellow.Modules.Sales.Application.Features.Categories;

internal sealed class GetCategories : IRequest<IEnumerable<CategoryDto>>;

internal sealed class GetCategoriesHandler : IRequestHandler<GetCategories, IEnumerable<CategoryDto>>
{
    private readonly SalesDbContext _context;
    private readonly IMemoryCache _memoryCache;

    public GetCategoriesHandler(SalesDbContext context, IMemoryCache memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache;
    }

    public async Task<IEnumerable<CategoryDto>> Handle(GetCategories request, CancellationToken cancellationToken)
    {
        if (_memoryCache.Get("Sales/Categories") is not null)
        {
            return _memoryCache.Get<IEnumerable<CategoryDto>>("Sales/Categories") ?? Array.Empty<CategoryDto>();
        }

        var categories = await _context.Categories.ToListAsync(cancellationToken);

        var categoriesDto = CategoryDto.BuildCategoryTree(categories);

        _memoryCache.Set("Sales/Categories", categoriesDto);

        return categoriesDto;
    }
}