using FluentValidation;
using MediatR;
using MarketNest.Application.Common.Exceptions;
using MarketNest.Application.Common.Interfaces;
using MarketNest.Domain.Interfaces.Repositories;
using MarketNest.Domain.Enums;

namespace MarketNest.Application.Categories;

public record DeleteCategoryCommand(Guid Id) : IRequest<Unit>;

public class DeleteCategoryValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly ICacheService _cache;

    public DeleteCategoryHandler(IUnitOfWork uow, ICacheService cache)
    {
        _uow = uow;
        _cache = cache;
    }

    public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var cat = await _uow.Categories.GetByIdAsync(request.Id);
        if (cat == null) throw new NotFoundException("Category not found");

        // check active listings
        var listings = await _uow.Listings.FindAsync(l => l.CategoryId == cat.Id && l.Status == ListingStatus.Active);
        if (listings.Any()) throw new ConflictException("Category has active listings");

        cat.IsActive = false;
        _uow.Categories.Update(cat);
        await _uow.SaveChangesAsync();
        await _cache.RemoveAsync("categories:tree");
        return Unit.Value;
    }
}
