using MongoDB.Bson;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Packages.Dtos;

public class GetActivePackagesByRiderIdResponseDto : IPackageDto
{
  public ObjectId Id { set; get; }
  public string Name { set; get; }
  public double Price { set; get; }
  public PackageType PackageType { set; get; }
  public DateTime StartDateTime { set; get; }
  public bool IsValid { set; get; }

}

