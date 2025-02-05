using Flurl;
using Flurl.Http;
using PaymentSystem.Infrastructure.Config;
using Rideshare.Application.Features.PaymentSystem.Application.Dtos;
using Rideshare.Application.Features.PaymentSystem.Application.DTOs;
using Microsoft.Extensions.Options;
using Rideshare.Application.Contracts.Infrastructure;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Text;



namespace Rideshare.Infrastructure.ChapaServices
{
    public class ChapaService : IChapaService
    {
        private readonly ChapaConfig _config;

        private readonly HttpClient _httpClient;

        public ChapaService(IOptions<ChapaConfig> config)
        {
            
                _config = config.Value;
                _httpClient = new HttpClient();
                Console.WriteLine($"{config.Value.ToString()} {config.Value == null}");
                if (string.IsNullOrEmpty(_config.API_SECRET))
                {
                    Console.WriteLine(_config);
                    throw new Exception("Secret Key can't be null");
                }    
        }


        public async Task<HttpResponseMessage> ProcessPaymentAsync(ChapaRequestDto requestDTO)
        {

            using (var _httpClient = new HttpClient())
            {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.chapa.co/v1/transaction/initialize");
            var reqDict = new Dictionary<string, object?>();
            
                reqDict.Add("email",requestDTO.Email);
                reqDict.Add("amount",requestDTO.Amount);
                reqDict.Add("first_name",requestDTO.FirstName);
                reqDict.Add("last_name", requestDTO.LastName);
                reqDict.Add("tx_ref",requestDTO.TransactionReference);
                reqDict.Add("currency","ETB");
                reqDict.Add("phone_number", requestDTO.PhoneNo);
                reqDict.Add("callback_url", requestDTO.CallbackUrl);
                reqDict.Add("return_url", requestDTO.ReturnUrl);
                reqDict.Add("customization[title]", requestDTO.CustomTitle);
                reqDict.Add("customization[description]", requestDTO.CustomDescription);
                reqDict.Add("customization[logo]", requestDTO.CustomLogo);
            

            request.Content = new StringContent(JsonSerializer.Serialize(reqDict), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _config.API_SECRET);
            var response = await _httpClient.SendAsync(request);
            return response;
            
            }
        }
  
    

        public async Task<ValidityReportDTO?> VerifyAsync(string txRef)
        {
            var Config = new ChapaConfig();
            try
            {
                var validityResponse = await $"https://api.chapa.co/v1/transaction/verify/{txRef}"
                        .WithHeader(ChapaConfig.AUTH_HEADER, $"Bearer {Config.API_SECRET}")
                        .GetJsonAsync<ValidityReportDTO>();
                return validityResponse;
            }
            catch (Exception)
            {
                return null;
            }
        }
}
}