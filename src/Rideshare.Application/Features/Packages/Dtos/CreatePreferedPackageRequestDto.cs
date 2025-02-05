using MongoDB.Bson;

namespace Rideshare.Application.Features.Packages.Dtos;

public class CreatePreferedPackageRequestDto
{ 
        public ObjectId PackageId {set; get;} 
        public ObjectId DriverId {set; get;}
}