using Microsoft.EntityFrameworkCore;
using Sellow.Modules.Sales.Core.Categories;

namespace Sellow.Modules.Sales.Infrastructure.DAL.Repositories;

internal sealed class CategoryRepository : ICategoryRepository
{
    private readonly SalesDbContext _context;

    public CategoryRepository(SalesDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsRootCategoryUnique(Category category, CancellationToken cancellationToken = default)
        => await _context.Categories
            .FirstOrDefaultAsync(x => x.Name == category.Name && x.ParentId == null, cancellationToken) is null;


    public async Task Save(Category category, CancellationToken cancellationToken = default)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Category?> Load(Guid id, CancellationToken cancellationToken = default)
        => await _context.Categories
            .Include(x => x.Subcategories)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
}