using Rideshare.Application.Features.Riders.Dtos.Common;

namespace Rideshare.Application.Features.Riders.Dtos;
public class UpdateRiderDto:BaseRiderDto
{
    public string FirstName { set; get; }
    public string LastName { set; get; }
    public string Email { set; get; }
    public string PhoneNumber {set;get;}
}