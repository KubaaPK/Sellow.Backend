using System.Net;
using Sellow.Shared.Abstractions.Exceptions;

namespace Sellow.Modules.Sales.Core.Categories;

internal sealed class Category
{
    public Guid Id { get; }
    public string Name { get; }

    public Guid? ParentId { get; }
    public Category? Parent { get; }

    private readonly IList<Category> _subcategories = new List<Category>();
    public IReadOnlyList<Category> Subcategories => _subcategories.AsReadOnly();

    public Category(string name)
    {
        Name = name;
    }

    public void AddSubcategory(Category subcategory)
    {
        if (_subcategories.Contains(subcategory))
        {
            throw new DuplicatedSubcategoryException(Name, subcategory.Name);
        }

        _subcategories.Add(subcategory);
    }

    private bool Equals(Category other)
    {
        return Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Category other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}

internal sealed class DuplicatedSubcategoryException : SellowException
{
    public override HttpStatusCode HttpCode => HttpStatusCode.Conflict;
    public override string ErrorCode => "duplicated_subcategory";

    public DuplicatedSubcategoryException(string parentCategoryName, string subcategoryName) : base(
        $"Category '{parentCategoryName}' has already subcategory '{subcategoryName}' defined.")
    {
    }
}