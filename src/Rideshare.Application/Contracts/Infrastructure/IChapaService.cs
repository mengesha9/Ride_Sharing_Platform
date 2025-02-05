using Microsoft.IdentityModel.Tokens;
using Rideshare.Application.Features.PaymentSystem.Application.Dtos;
using Rideshare.Application.Features.PaymentSystem.Application.DTOs;
using System.Threading.Tasks;
using Rideshare.Domain.Entities;

namespace Rideshare.Application.Contracts.Infrastructure
{
    public interface IChapaService
    {

        Task<HttpResponseMessage> ProcessPaymentAsync(ChapaRequestDto requestDto);
        Task<ValidityReportDTO?> VerifyAsync(string txRef);
    }

}