using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sellow.Modules.Sales.Application.Features.Categories;
using Sellow.Shared.Infrastructure.Api;

namespace Sellow.Modules.Sales.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/sales/categories")]
[ApiVersion("1.0")]
internal sealed class CategoryController : ControllerBase
{
    private readonly ISender _sender;

    public CategoryController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Adds a new category.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /sales/categories
    ///     {
    ///        "name": "Elektronika",
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Category has been successfully added.</response>
    /// <response code="400">Request body validation has failed.</response>
    /// <response code="404">Parent category with given id was not found.</response>
    /// <response code="409">Root category or subcategory already exists.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost]
    [ProducesResponseType(201)]
    public async Task<IResult> AddCategory([FromBody] AddCategory command,
        CancellationToken cancellationToken = default)
    {
        var categoryId = await _sender.Send(command, cancellationToken);

        return Results.Created($"{Request.GetActionUrl()}/{categoryId}", null);
    }

    /// <summary>
    /// Gets all sales categories.
    /// </summary>
    /// <response code="200">List with the categories tree.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoryDto>), 200, "application/json")]
    public async Task<IResult> GetCategories(CancellationToken cancellationToken = default)
    {
        var categories = await _sender.Send(new GetCategories(), cancellationToken);
        return Results.Ok(categories);
    }

    /// <summary>
    /// Get a single category
    /// </summary>
    /// <response code="200">A single category.</response>
    /// <response code="404">Category was not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CategoryDto), 200, "application/json")]
    public async Task<IResult> GetCategory(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _sender.Send(new GetCategory(id), cancellationToken);
        return Results.Ok(category);
    }
}