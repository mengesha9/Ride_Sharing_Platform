using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.PackageTypes.CQRS.Requests.Commands
{
    public class UpdatePackageTypeCommand : IRequest<BaseCommandResponse<PackageTyp>>
    {
        public int Id { get; set; }
        public string? type { get; set; }
        public int? NumberOfDay { get; set; }
        public double? Discount { get; set; }
    }
}