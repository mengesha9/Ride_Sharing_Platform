using MediatR;
using Rideshare.Application.Responses;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.PackageTypes.CQRS.Requests.Commands;
public class CreatePackageTypeCommand : IRequest<BaseCommandResponse<PackageTyp>>
{
    public int Id { get; set; }
    public string type { get; set; }
    public int NumberOfDay { get; set; }
    public Double Discount { get; set; }
}