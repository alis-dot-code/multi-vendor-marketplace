using FluentValidation;
using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Common.Exceptions;
using MarketNest.Application.Common.Interfaces;
using MarketNest.Domain.Interfaces.Repositories;
using AutoMapper;
using MarketNest.Domain.Entities;

namespace MarketNest.Application.Categories;

public record CreateCategoryCommand(string Name, string Slug, string? Description, Guid? ParentId, int SortOrder) : IRequest<CategoryDto>;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Slug).NotEmpty();
    }
}

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICacheService _cache;
    private readonly IMapper _mapper;

    public CreateCategoryHandler(IUnitOfWork uow, ICacheService cache, IMapper mapper)
    {
        _uow = uow;
        _cache = cache;
        _mapper = mapper;
    }

    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var existing = (await _uow.Categories.FindAsync(c => c.Slug == request.Slug)).FirstOrDefault();
        if (existing != null) throw new ConflictException("Slug already exists");

        var cat = new Category
        {
            Name = request.Name,
            Slug = request.Slug,
            Description = request.Description,
            ParentId = request.ParentId,
            SortOrder = request.SortOrder,
            IsActive = true
        };

        await _uow.Categories.AddAsync(cat);
        await _uow.SaveChangesAsync();
        await _cache.RemoveAsync("categories:tree");
        return _mapper.Map<CategoryDto>(cat);
    }
}
