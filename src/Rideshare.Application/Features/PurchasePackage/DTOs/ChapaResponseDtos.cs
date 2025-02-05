using System.Transactions;
using MongoDB.Bson;
using System.Net;
namespace Rideshare.Application.Features.PaymentSystem.Application.Dtos
{
    public class ChapaResponseDto
    {
        public ObjectId? UserId { set; get; }
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }
        public string CheckoutUrl { get; set; }
    }
}