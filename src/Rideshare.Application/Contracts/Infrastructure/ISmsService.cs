namespace Rideshare.Application.Contracts.Infrastructure;

public interface ISmsService 
{
     public Task<bool> SendSMS(string phone, string msg);
}
