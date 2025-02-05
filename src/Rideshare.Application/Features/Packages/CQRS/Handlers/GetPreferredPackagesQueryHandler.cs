using AutoMapper;
using MediatR;
using MongoDB.Bson;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.Packages.CQRS.Requests;
using Rideshare.Application.Features.Packages.Dtos;

namespace Rideshare.Application.Features.Drivers.CQRS.Handlers
{
    public class GetPreferredPackagesQueryHandler : IRequestHandler<GetPreferredPackagesQuery, List<PreferredPackagesResponseDto>>
    { 
        private readonly IDriverRepository _driverRepository;
        private readonly IMapper _mapper;
        public GetPreferredPackagesQueryHandler(IDriverRepository driverRepository, IMapper mapper)
        {
            _driverRepository = driverRepository;
            _mapper = mapper;
        }
        public async Task<List<PreferredPackagesResponseDto>> Handle(GetPreferredPackagesQuery request, CancellationToken cancellationToken)
        {
            var packages = await  _driverRepository.GetPreferredPackages(request.DriverId);
            return _mapper.Map<List<PreferredPackagesResponseDto>>(packages);
        }
    }
}
