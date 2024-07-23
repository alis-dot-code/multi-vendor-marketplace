using FluentValidation;
using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Common.Exceptions;
using MarketNest.Application.Common.Interfaces;
using MarketNest.Domain.Interfaces.Repositories;
using AutoMapper;
using MarketNest.Domain.Entities;

namespace MarketNest.Application.Categories;

public record UpdateCategoryCommand(Guid Id, string Name, string Slug, string? Description, Guid? ParentId, int SortOrder, bool IsActive) : IRequest<CategoryDto>;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Slug).NotEmpty();
    }
}

public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICacheService _cache;
    private readonly IMapper _mapper;

    public UpdateCategoryHandler(IUnitOfWork uow, ICacheService cache, IMapper mapper)
    {
        _uow = uow;
        _cache = cache;
        _mapper = mapper;
    }

    public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var cat = await _uow.Categories.GetByIdAsync(request.Id);
        if (cat == null) throw new NotFoundException("Category not found");

        cat.Name = request.Name;
        cat.Slug = request.Slug;
        cat.Description = request.Description;
        cat.ParentId = request.ParentId;
        cat.SortOrder = request.SortOrder;
        cat.IsActive = request.IsActive;

        _uow.Categories.Update(cat);
        await _uow.SaveChangesAsync();
        await _cache.RemoveAsync("categories:tree");
        return _mapper.Map<CategoryDto>(cat);
    }
}
