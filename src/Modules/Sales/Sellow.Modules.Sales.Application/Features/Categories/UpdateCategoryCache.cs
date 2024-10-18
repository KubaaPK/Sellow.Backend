using MediatR;
using Microsoft.Extensions.Logging;
using Sellow.Modules.Sales.Application.Services;

namespace Sellow.Modules.Sales.Application.Features.Categories;

internal sealed record CategoriesUpdated() : INotification;

internal sealed class UpdateCategoryCache : INotificationHandler<CategoriesUpdated>
{
    private readonly ILogger<UpdateCategoryCache> _logger;
    private readonly ICategoryCacheService _categoryCacheService;

    public UpdateCategoryCache(ILogger<UpdateCategoryCache> logger, ICategoryCacheService categoryCacheService)
    {
        _logger = logger;
        _categoryCacheService = categoryCacheService;
    }

    public async Task Handle(CategoriesUpdated notification, CancellationToken cancellationToken)
    {
        await _categoryCacheService.UpdateCategoriesInCache(cancellationToken);

        _logger.LogInformation("Categories cache has been updated");
    }
}