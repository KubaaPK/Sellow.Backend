using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Sellow.Modules.Sales.Infrastructure.DAL;

namespace Sellow.Modules.Sales.Application.Features.Categories;

internal sealed record CategoriesUpdated() : INotification;

internal sealed class UpdateCategoryCache : INotificationHandler<CategoriesUpdated>
{
    private readonly ILogger<UpdateCategoryCache> _logger;
    private readonly SalesDbContext _context;
    private readonly IMemoryCache _memoryCache;

    public UpdateCategoryCache(ILogger<UpdateCategoryCache> logger, SalesDbContext context, IMemoryCache memoryCache)
    {
        _logger = logger;
        _context = context;
        _memoryCache = memoryCache;
    }

    public async Task Handle(CategoriesUpdated notification, CancellationToken cancellationToken)
    {
        var categories = await _context.Categories.ToListAsync(cancellationToken);

        var categoriesDto = CategoryDto.BuildCategoryTree(categories);

        _memoryCache.Set("Sales/Categories", categoriesDto);

        _logger.LogInformation("Categories cache has been updated");
    }
}