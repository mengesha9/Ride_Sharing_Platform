using System.Text;
using Newtonsoft.Json;
using Rideshare.Application.Contracts.Infrastructure;

public class SmsService : ISmsService
{
  private readonly HttpClient _client;
  private readonly string _smsUrl = Environment.GetEnvironmentVariable("SMS_URL");
  private readonly string _smsToken = Environment.GetEnvironmentVariable("SMS_TOKEN");

  public SmsService(HttpClient client)
  {
    _client = client;
  }

  public async Task<bool> SendSMS(string phone, string msg)
  {
    var data = new
    {
      msg = msg,
      phone = phone,
      token = _smsToken
    };

    var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");


    var response = await _client.PostAsync(_smsUrl, content);
    var responseString = await response.Content.ReadAsStringAsync();
    var responseObject = JsonConvert.DeserializeObject<dynamic>(responseString);

    bool result;
    bool.TryParse(responseObject.error.ToString(), out result);
    return !result;
  }
}
