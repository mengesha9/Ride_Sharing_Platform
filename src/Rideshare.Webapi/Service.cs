// Startup.cs (ConfigureServices method)
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentSystem.Infrastructure.Config;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Infrastructure.ChapaServices;
using Rideshare.Infrastructure.Persistence.Repositories;
using Rideshare.WebApi.Services;


public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        // Add other services and configurations
        services.AddTransient<IPaymentRepository, PaymentRepository>();
        services.Configure<ChapaConfig>(Configuration.GetSection("ChapaConfig"));
        services.AddScoped<IChapaService, ChapaService>();
        services.AddScoped<IUserAccessor, UserAccessor>();

    }

}