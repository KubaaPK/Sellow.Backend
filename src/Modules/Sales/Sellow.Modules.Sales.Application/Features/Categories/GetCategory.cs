using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Sellow.Modules.Sales.Application.Features.Categories.Exceptions;
using Sellow.Modules.Sales.Infrastructure.DAL;

namespace Sellow.Modules.Sales.Application.Features.Categories;

internal sealed record GetCategory(Guid Id) : IRequest<CategoryDto>;

internal sealed class GetCategoryHandler : IRequestHandler<GetCategory, CategoryDto>
{
    private readonly SalesDbContext _context;
    private readonly IMemoryCache _memoryCache;

    public GetCategoryHandler(SalesDbContext context, IMemoryCache memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache;
    }

    public async Task<CategoryDto> Handle(GetCategory request, CancellationToken cancellationToken)
    {
        if (_memoryCache.Get("Sales/Categories") is not null)
        {
            var cachedCategories = _memoryCache
                .Get<IEnumerable<CategoryDto>>("Sales/Categories") ?? new List<CategoryDto>();
            var foundCachedCategory = CategoryDto.FindById(cachedCategories, request.Id);

            if (foundCachedCategory is null)
            {
                throw new CategoryNotFoundException(request.Id);
            }

            return foundCachedCategory;
        }

        var categories = await _context.Categories.ToListAsync(cancellationToken);

        var categoriesDto = CategoryDto.BuildCategoryTree(categories);

        var foundCategory = CategoryDto.FindById(categoriesDto, request.Id);

        if (foundCategory is null)
        {
            throw new CategoryNotFoundException(request.Id);
        }

        return foundCategory;
    }
}