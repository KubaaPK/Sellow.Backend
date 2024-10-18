using Sellow.Modules.Sales.Application.Features.Categories;

namespace Sellow.Modules.Sales.Application.Services;

internal interface ICategoryCacheService
{
    /// <summary>Gets the categories from the cache or if there is no categories cached load from the database.</summary>
    Task<IEnumerable<CategoryDto>> GetCategories(CancellationToken cancellationToken = default);

    Task UpdateCategoriesInCache(CancellationToken cancellationToken = default);
}