using AutoMapper;
using MediatR;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.PackageTypes.CQRS.Requests.Queries;
using Rideshare.Application.Features.PackageTypes.Dtos;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rideshare.Application.Features.PackageTypes.CQRS.Handlers.Queries
{
    public class GetPackageTypeRequestHandler : IRequestHandler<GetPackageTypesRequest,BaseListResponse<PackageTypeDto>>
    {
        private readonly IPackageTypeRepository _packageTypeRepository;
        private readonly IMapper _mapper;

        public GetPackageTypeRequestHandler(IPackageTypeRepository packageTypeRepository, IMapper mapper)
        {
            _packageTypeRepository = packageTypeRepository;
            _mapper = mapper;
        }

        public async Task<BaseListResponse<PackageTypeDto>> Handle(GetPackageTypesRequest request, CancellationToken cancellationToken)
        {
            var packageTypes = await _packageTypeRepository.GetAll();
            var packageTypeDtos = _mapper.Map<List<PackageTypeDto>>(packageTypes);
            var response = new BaseListResponse<PackageTypeDto>();
            response.Value = packageTypeDtos;
            response.Succeeded = true;
            return response;
        }
    }
}