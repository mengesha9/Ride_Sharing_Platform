using MongoDB.Bson;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Features.Payment.Dtos;

public class GetTransactionHistoryResponseDto
{
    public ObjectId Id { set; get; }
    public ObjectId PackageId { set; get; }

    public ObjectId UserId { set; get; }

    public double Amount { get; set; }

    public string Email { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string TransactionReference { get; set; } = string.Empty;

    public string? PhoneNo { get; set; }

    public string? Currency { get; set; }

    public string? CallbackUrl { get; set; }

    public string? ReturnUrl { get; set; }

    public string? CustomTitle { get; set; }

    public string? CustomDescription { get; set; }

    public string? CustomLogo { get; set; }
    public bool? IsSuccessPayment { get; set; }

}
