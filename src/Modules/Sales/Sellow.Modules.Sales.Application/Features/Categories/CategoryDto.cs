using Sellow.Modules.Sales.Core.Categories;

namespace Sellow.Modules.Sales.Application.Features.Categories;

internal sealed record CategoryDto(Guid Id, string Name, Guid? ParentId, IEnumerable<CategoryDto> Subcategories)
{
    public static List<CategoryDto> BuildCategoryTree(List<Category> categories)
    {
        var categoryLookup = categories.ToDictionary(c => c.Id);

        return (from category in categories
            where category.ParentId == null
            select MapToCategoryDto(category)).ToList();
    }

    private static CategoryDto MapToCategoryDto(Category category)
    {
        var subcategories = category.Subcategories
            .Select(MapToCategoryDto)
            .ToList();

        return new CategoryDto(
            category.Id,
            category.Name,
            category.ParentId,
            subcategories
        );
    }
}