
using MongoDB.Bson;
using Rideshare.Domain.Common;

namespace Rideshare.Domain.Entities;

public class GetRiderHistoryResponseDto
{
    public ObjectId PackageId {set; get;}
    public ObjectId RiderId {set; get;}
    public required string Name {set; get;}
    public required Location PickUpLocation {set; get;}
    public required Location DropOffLocation {set; get;}
    public double Distance {set; get;}
    public DateTime StartDateTime {set; get;}
    public bool IsConfirmed {set; get;} = false;
    // public int RatingId {set; get;}
    // public string PaymentType {set; get;}
    // public string TransactionId {set; get;}
    // public double Distance {set; get;}
    
    public required Package Package {set; get;}

}
