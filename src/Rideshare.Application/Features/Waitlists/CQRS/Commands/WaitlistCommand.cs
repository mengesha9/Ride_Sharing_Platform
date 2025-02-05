using MediatR;
using Rideshare.Application.Features.Waitlists.Dtos;

namespace RideShare.Application.Features.Waitlists.CQRS.Commands
{
    public class WaitlistCommand : IRequest<Unit>
    {
        public WaitlistRequestDto WaitlistRequestDto { get; set; }
    }
}
