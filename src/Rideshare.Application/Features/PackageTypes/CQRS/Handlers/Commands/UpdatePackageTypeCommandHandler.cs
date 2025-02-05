using AutoMapper;
using MediatR;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.PackageTypes.CQRS.Requests.Commands;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;


namespace Rideshare.Application.Features.PackageTypes.CQRS.Handlers.Commands
{
    public class UpdatePackageTypeCommandHandler : IRequestHandler<UpdatePackageTypeCommand, BaseCommandResponse<PackageTyp>>
    {
        private readonly IPackageTypeRepository _packageTypeRepository;
        private readonly IMapper _mapper;

        public UpdatePackageTypeCommandHandler(IPackageTypeRepository packageTypeRepository, IMapper mapper)
        {
            _packageTypeRepository = packageTypeRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse<PackageTyp>> Handle(UpdatePackageTypeCommand request, CancellationToken cancellationToken)
        {
            var existingPackageType = await _packageTypeRepository.Get(request.Id);

            if (existingPackageType == null)
            {
                return new BaseCommandResponse<PackageTyp> { Succeeded = false, Message = "Package Type not found" };
            }
            var updated=new PackageTyp{
                Id=request.Id,
                type=request.type??existingPackageType.type,
                NumberOfDay=request.NumberOfDay??existingPackageType.NumberOfDay,
                Discount=request.Discount??existingPackageType.Discount
            };
            try{
                await _packageTypeRepository.Update(updated);
                var response = new BaseCommandResponse<PackageTyp>();
                response.Succeeded = true;
                return response;
            }catch{
                return new BaseCommandResponse<PackageTyp> { Succeeded = false, Message = "An error occurred while updating the package type" };
            }
        }
    }
}