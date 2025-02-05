using MongoDB.Bson;

namespace Rideshare.Application.Features.PaymentSystem.Application.Dtos;

public class ChapaRequestDto
{
    public string? Email { get; set; }
    public float Amount { get; set; }
    public string? FirstName { get; set; }
    public ObjectId PackageId { set; get; }

    public string? LastName { get; set; }
    public string? TransactionReference { get; set; }
    public string? Currency { get; set; }
    public string? PhoneNo { get; set; }
    public string? CallbackUrl { get; set; }
    public string? ReturnUrl { get; set; }
    public string? CustomDescription { get; set; }
    public string? CustomLogo { get; set; }
    public string? CustomTitle { get; set; }
}