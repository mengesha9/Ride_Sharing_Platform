
using MongoDB.Bson;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.Packages.Dtos;

public class GetPackageRequestDto 
{
    public ObjectId Id { set; get;}
}
