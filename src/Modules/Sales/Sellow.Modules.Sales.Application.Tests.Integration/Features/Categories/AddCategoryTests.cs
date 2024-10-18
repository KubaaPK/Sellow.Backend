using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sellow.Modules.Sales.Application.Features.Categories;
using Sellow.Modules.Sales.Core.Categories;
using Sellow.Modules.Sales.Infrastructure.DAL.Repositories;

namespace Sellow.Modules.Sales.Application.Tests.Integration.Features.Categories;

public sealed class AddCategoryTests : IDisposable
{
    private Task<Guid> Act(AddCategory command) => _handler.Handle(command, default);

    [Fact]
    internal async Task should_add_a_new_root_category()
    {
        await _testDatabase.Init();
        var command = new AddCategory("Elektronika", null);

        await Act(command);

        Assert.Equal(1, _testDatabase.Context.Categories.Count());
    }

    [Fact]
    internal async Task should_not_allow_to_add_duplicated_root_category()
    {
        await _testDatabase.Init();
        var command = new AddCategory("Elektronika", null);
        await Act(command);

        await Assert.ThrowsAsync<DuplicatedRootCategoryException>(() => Act(command));
    }

    [Fact]
    internal async Task should_add_a_new_subcategory()
    {
        await _testDatabase.Init();
        var parentCategory = new AddCategory("Elektronika", null);
        var parentId = await Act(parentCategory);
        var command = new AddCategory("Smartfony", parentId);
        await Act(command);

        Assert.Equal(2, _testDatabase.Context.Categories.Count());
    }

    [Fact]
    internal async Task should_not_allow_to_add_duplicated_subcategory()
    {
        await _testDatabase.Init();
        var parentCategoryId = await Act(new AddCategory("Elektronika", null));
        var command = new AddCategory("Smartfony", parentCategoryId);
        await Act(command);

        await Assert.ThrowsAsync<DuplicatedSubcategoryException>(() => Act(command));
    }

    [Fact]
    internal async Task should_publish_an_update_cache_event_when_category_has_been_added()
    {
        await _testDatabase.Init();
        var command = new AddCategory("Elektronika", null);
        await Act(command);

        await _publisher.Received(1).Publish(Arg.Any<CategoriesUpdated>());
    }

    #region Arrange

    private readonly TestDatabase _testDatabase;
    private readonly AddCategoryHandler _handler;
    private readonly IPublisher _publisher;

    public AddCategoryTests()
    {
        _testDatabase = new TestDatabase();
        _publisher = Substitute.For<IPublisher>();
        ICategoryRepository categoryRepository = new CategoryRepository(_testDatabase.Context);
        _handler = new AddCategoryHandler(Substitute.For<ILogger<AddCategoryHandler>>(), categoryRepository,
            _publisher);
    }

    #endregion

    public void Dispose() => _testDatabase.Dispose();
}