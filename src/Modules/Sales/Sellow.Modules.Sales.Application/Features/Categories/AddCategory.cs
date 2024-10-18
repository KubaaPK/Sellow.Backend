using System.Net;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Sellow.Modules.Sales.Application.Features.Categories.Exceptions;
using Sellow.Modules.Sales.Core.Categories;
using Sellow.Shared.Abstractions.Exceptions;

namespace Sellow.Modules.Sales.Application.Features.Categories;

internal sealed record AddCategory(string Name, Guid? ParentId) : IRequest<Guid>;

internal sealed class AddCategoryValidator : AbstractValidator<AddCategory>
{
    public AddCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required.");
    }
}

internal sealed class AddCategoryHandler : IRequestHandler<AddCategory, Guid>
{
    private readonly ILogger<AddCategoryHandler> _logger;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IPublisher _publisher;

    public AddCategoryHandler(ILogger<AddCategoryHandler> logger, ICategoryRepository categoryRepository,
        IPublisher publisher)
    {
        _logger = logger;
        _categoryRepository = categoryRepository;
        _publisher = publisher;
    }

    public async Task<Guid> Handle(AddCategory request, CancellationToken cancellationToken)
        => request.ParentId is not null
            ? await AddSubcategory(request, cancellationToken)
            : await AddRootCategory(request, cancellationToken);


    private async Task<Guid> AddRootCategory(AddCategory request, CancellationToken cancellationToken)
    {
        var category = new Category(request.Name);

        var isRootCategoryUnique = await _categoryRepository.IsRootCategoryUnique(category, cancellationToken);

        if (isRootCategoryUnique is false)
        {
            throw new DuplicatedRootCategoryException();
        }

        await _categoryRepository.Save(category, cancellationToken);

        _logger.LogInformation("Category {@Category} has been added", category);

        await _publisher.Publish(new CategoriesUpdated(), cancellationToken);

        return category.Id;
    }

    private async Task<Guid> AddSubcategory(AddCategory request, CancellationToken cancellationToken)
    {
        var parent = await _categoryRepository.Load((Guid)request.ParentId!, cancellationToken);
        if (parent is null)
        {
            throw new CategoryNotFoundException((Guid)request.ParentId);
        }

        var subcategory = new Category(request.Name);

        parent.AddSubcategory(subcategory);

        await _categoryRepository.Save(parent, cancellationToken);

        _logger.LogInformation("Category {@Category} has been added", subcategory);

        await _publisher.Publish(new CategoriesUpdated(), cancellationToken);

        return subcategory.Id;
    }
}

internal sealed class DuplicatedRootCategoryException : SellowException
{
    public override HttpStatusCode HttpCode => HttpStatusCode.Conflict;
    public override string ErrorCode => "duplicated_root_category";

    public DuplicatedRootCategoryException() : base("Duplicated root category.")
    {
    }
}