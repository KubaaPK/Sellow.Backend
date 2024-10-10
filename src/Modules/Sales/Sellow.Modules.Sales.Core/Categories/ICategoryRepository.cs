namespace Sellow.Modules.Sales.Core.Categories;

internal interface ICategoryRepository
{
    Task<bool> IsRootCategoryUnique(Category category, CancellationToken cancellationToken = default);
    Task Save(Category category, CancellationToken cancellationToken = default);
    Task<Category?> Load(Guid id, CancellationToken cancellationToken = default);
}