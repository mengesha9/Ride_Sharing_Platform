using MongoDB.Bson;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.Riders.Dtos;

public class GetRiderHistoryRequestDto
{
    public ObjectId? RiderId { set; get; }

    // sort by
    public SortField SortField { set; get; } = SortField.StartDateTime;
    public bool IsAscending { set; get; } = false;

    public double MinPrice { set; get; } = 0;
    public double MaxPrice { set; get; } = double.MaxValue;
    public DateOnly StartDate { set; get; } = DateOnly.MinValue;
    public DateOnly EndDate { set; get; } = DateOnly.MaxValue;

}