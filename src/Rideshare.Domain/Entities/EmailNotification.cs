using MongoDB.Bson;
using Rideshare.Domain.Common;

namespace Rideshare.Domain.Entities;

public class EmailNotification : BaseEntity
{
    public ObjectId RiderId { set; get; }
    public bool IsEnabled { set; get; } = true;
}
