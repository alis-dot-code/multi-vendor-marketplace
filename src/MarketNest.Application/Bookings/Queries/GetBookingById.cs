using MediatR;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Common.Exceptions;
using MarketNest.Domain.Interfaces.Repositories;
using AutoMapper;

namespace MarketNest.Application.Bookings.Queries;

public record GetBookingByIdQuery(Guid Id) : IRequest<BookingDto>;

public class GetBookingByIdHandler : IRequestHandler<GetBookingByIdQuery, BookingDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetBookingByIdHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<BookingDto> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        var booking = await _uow.Bookings.GetByIdAsync(request.Id) ?? throw new NotFoundException("Booking not found");
        return _mapper.Map<BookingDto>(booking);
    }
}
