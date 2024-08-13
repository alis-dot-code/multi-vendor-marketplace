using System;
using System.Threading;
using System.Threading.Tasks;
using MarketNest.Application.Common.Interfaces;
using MediatR;
using MarketNest.Application.Common.Models;
using MarketNest.Application.Common.DTOs;
using AutoMapper;
using MarketNest.Domain.Interfaces.Repositories;
using System.Linq;

namespace MarketNest.Application.Reviews.Queries
{
    public static class GetReviewsByListing
    {
        public record Query(Guid ListingId, int Page, int PageSize) : IRequest<PagedResult<ReviewDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<ReviewDto>>
        {
            private readonly IUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<PagedResult<ReviewDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var page = Math.Max(1, request.Page);
                var pageSize = Math.Clamp(request.PageSize, 1, 100);
                var paged = await _uow.Reviews.GetByListingAsync(request.ListingId, page, pageSize);
                var dtos = paged.Items.Select(r => _mapper.Map<ReviewDto>(r)).ToList();
                return new PagedResult<ReviewDto>(dtos, paged.TotalCount, page, pageSize);
            }
        }
    }
}
