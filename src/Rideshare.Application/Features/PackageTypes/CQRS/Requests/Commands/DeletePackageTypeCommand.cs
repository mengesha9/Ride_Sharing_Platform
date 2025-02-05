using MediatR;
using Rideshare.Application.Responses;

namespace Rideshare.Application.Features.PackageTypes.CQRS.Requests.Commands
{
    public class DeletePackageTypeCommand : IRequest<BaseCommandResponse<bool>>
    {
        public int Id { get; set; }
    }
}