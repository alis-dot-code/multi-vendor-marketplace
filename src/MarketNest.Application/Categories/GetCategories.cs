using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Common.Interfaces;
using AutoMapper;
using MarketNest.Domain.Interfaces.Repositories;
using MarketNest.Domain.Entities;

namespace MarketNest.Application.Categories;

public record GetCategoriesQuery() : IRequest<List<CategoryDto>>;

public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICacheService _cache;
    private readonly IMapper _mapper;

    public GetCategoriesHandler(IUnitOfWork uow, ICacheService cache, IMapper mapper)
    {
        _uow = uow;
        _cache = cache;
        _mapper = mapper;
    }

    public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var key = "categories:tree";
        var cached = await _cache.GetAsync<List<CategoryDto>>(key);
        if (cached != null) return cached;

        var cats = (await _uow.Categories.GetAllAsync()).ToList();
        var lookup = cats.ToLookup(c => c.ParentId);

        List<CategoryDto> Build(Guid? parentId)
        {
            return lookup[parentId]
                .OrderBy(c => c.SortOrder)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Slug = c.Slug,
                    IconUrl = c.IconUrl,
                    Children = Build(c.Id)
                }).ToList();
        }

        var tree = Build(null);
        await _cache.SetAsync(key, tree, TimeSpan.FromHours(1));
        return tree;
    }
}
