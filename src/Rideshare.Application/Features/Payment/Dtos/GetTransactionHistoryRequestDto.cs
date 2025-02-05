
using MongoDB.Bson;
using Rideshare.Domain.Common;

namespace Rideshare.Application.Features.Payment.Dtos;

public class GetTransactionHistoryRequestDto
{
    public ObjectId RiderId { get; set; }
}