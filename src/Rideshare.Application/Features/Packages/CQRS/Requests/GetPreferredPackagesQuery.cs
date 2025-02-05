using MediatR;
using MongoDB.Bson;
using Rideshare.Application.Features.Packages.Dtos;

namespace Rideshare.Application.Features.Packages.CQRS.Requests
{
    public class GetPreferredPackagesQuery : IRequest<List<PreferredPackagesResponseDto>>
    {
        public ObjectId DriverId { get; set; }
    }
}
