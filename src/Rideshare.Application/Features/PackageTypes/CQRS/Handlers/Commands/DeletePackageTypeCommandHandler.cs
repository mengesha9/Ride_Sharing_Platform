using MediatR;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Application.Features.PackageTypes.CQRS.Requests.Commands;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.PackageTypes.CQRS.Handlers.Commands
{
    public class DeletePackageTypeCommandHandler : IRequestHandler<DeletePackageTypeCommand, BaseCommandResponse<bool>>
    {
        private readonly IPackageTypeRepository _packageTypeRepository;

        public DeletePackageTypeCommandHandler(IPackageTypeRepository packageTypeRepository)
        {
            _packageTypeRepository = packageTypeRepository;
        }

        public async Task<BaseCommandResponse<bool>> Handle(DeletePackageTypeCommand request, CancellationToken cancellationToken)
        {
            var packageType = await _packageTypeRepository.Get(request.Id);
            if (packageType == null)
            {
                return new BaseCommandResponse<bool> { Succeeded = false, Message = "Package Type not found" };
            }
            await _packageTypeRepository.Delete(packageType);
            return new BaseCommandResponse<bool> { Succeeded = true ,Message="Package Type deleted successfully"};
        }
    }
   
}