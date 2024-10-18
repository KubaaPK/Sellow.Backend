using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Sellow.Modules.Sales.Application.Features.Categories;
using Sellow.Modules.Sales.Infrastructure.DAL;

namespace Sellow.Modules.Sales.Application.Services;

internal sealed class CategoryCacheService : ICategoryCacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly SalesDbContext _context;

    public CategoryCacheService(IMemoryCache memoryCache, SalesDbContext context)
    {
        _memoryCache = memoryCache;
        _context = context;
    }

    public async Task<IEnumerable<CategoryDto>> GetCategories(CancellationToken cancellationToken = default)
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

    public async Task UpdateCategoriesInCache(CancellationToken cancellationToken = default)
    {
        var categories = await _context.Categories.ToListAsync(cancellationToken);
        var categoriesDto = CategoryDto.BuildCategoryTree(categories);
        _memoryCache.Set("Sales/Categories", categoriesDto);
    }
}