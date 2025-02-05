using System.Text;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentSystem.Infrastructure.Config;
using Rideshare.Application.Contracts.Identity.ConfigurationModels;
using Rideshare.Application.Contracts.Identity.Services;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Infrastructure.Identity.Services;
using Rideshare.Infrastructure.ChapaServices;
using Rideshare.Infrastructure.PushNotifications;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Infrastructure.Persistence.Repositories;
using Rideshare.Domain.Common;
using MongoDB.Driver;
using Rideshare.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Rideshare.Infrastructure.Persistance.Repositories;

namespace Rideshare.Infrastructure;

public static class InfrastrucureServiceRegistration
{


  public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddHttpClient<ISmsService, SmsService>();
    GoogleCredential credential = GoogleCredential.FromFile("./firebaseServiceAccountKey.json");
    FirebaseApp.Create(new AppOptions
    {
      Credential = credential
    });

    // Register Services
    services.AddScoped<INotificationService, FirebaseNotificationService>();
    services.AddScoped<IMapService, MapService>();
    services.AddHttpClient<ISmsService, SmsService>();
    services.AddScoped<IAuthService, AuthService>();
    services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
    services.AddScoped<IImageUploadService, ImageUploadService>();
    services.Configure<ChapaConfig>(configuration.GetSection("ChapaConfig"));

    services.AddScoped<IChapaService, ChapaService>();

    return services;
  }

  public static async Task<IServiceCollection> AddPersistenceAsync(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddSingleton<IMongoClient>(sp =>
    {
      var connectionString = configuration.GetConnectionString("MongoConnectionString");
      return new MongoClient(connectionString);
    });
    var databaseName = configuration.GetValue<string>("MongoDatabaseName");
    services.AddScoped(sp =>
    {
      var mongoClient = sp.GetRequiredService<IMongoClient>();
      var database = mongoClient.GetDatabase(databaseName);
      var collectionNames = database.ListCollectionNames().ToList();
      if (!collectionNames.Contains("Package"))
      {
        database.CreateCollection("Package");
      };
      if (!collectionNames.Contains("RiderHistory"))
      {
        database.CreateCollection("RiderHistory");
      };
      if (!collectionNames.Contains("RiderLocation"))
      {
        database.CreateCollection("RiderLocation");
      };
      if (!collectionNames.Contains("DriverHistory"))
      {
        database.CreateCollection("DriverHistory");
      };
      if (!collectionNames.Contains("Otp"))
      {
        database.CreateCollection("Otp");
      };
      if (!collectionNames.Contains("Driver"))
      {
        database.CreateCollection("Driver");
      };
      if (!collectionNames.Contains("EmailNotification"))
      {
        database.CreateCollection("EmailNotification");
      };
      if (!collectionNames.Contains("PackageTypes"))
      {
        database.CreateCollection("PackageTypes");
      };
      return database;
    });
    var packageTypeMappings = new Dictionary<PackageType, int>
        {
            { PackageType.Onetime, 0 },
            { PackageType.Weekly, 1 },
            { PackageType.Monthly, 2 },
            { PackageType.Quarterly, 3 }
        };
    var vehicleTypeMappings = new Dictionary<VehicleType, int>
        {
            { VehicleType.Economy, 0 },
            { VehicleType.Classic, 1 },
            { VehicleType.Minivan, 2 },
            { VehicleType.Minibus, 3 },
            { VehicleType.Lada, 4 },
        };


    services.AddScoped<IEnumRepository<PackageType>>(sp => new EnumRepository<PackageType>(packageTypeMappings));
    services.AddScoped<IEnumRepository<VehicleType>>(sp => new EnumRepository<VehicleType>(vehicleTypeMappings));
    services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    services.AddScoped<IRiderRepository, RiderRepository>();
    services.AddScoped<IOtpRepository, OtpRepository>();
    services.AddScoped<IPackageRepository, PackageRepository>();
    services.AddScoped<IRiderHistoryRepository, RiderHistoryRepository>();
    services.AddScoped<IRiderLocationRepository, RiderLocationRepository>();
    services.AddScoped<IDriverHistoryRepository, DriverHistoryRepository>();
    services.AddScoped<IDriverRepository, DriverRepository>();
    services.AddScoped<IAdminRepository, AdminRepository>();
    services.AddScoped<IWaitlistRepository, WaitlistRepository>();
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddScoped<IRiderNotificationRepository, RiderNotificationRepository>();
    services.AddScoped<IPaymentRepository, PaymentRepository>();
    services.AddScoped<IEmailNotificationRepository, EmailNotificationRepository>();
    services.AddScoped<IPackageTypeRepository, PackageTypeRepository>();

    return services;
  }
  public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
  {
    #region Identity Services
    services.AddTransient<IAuthService, AuthService>();
    services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
    #endregion

    #region Identity Services
    var identityDbSettings = configuration.GetSection(IdentityDbSettings.SectionName).Get<IdentityDbSettings>();
    services
      .AddIdentity<ApplicationUser, ApplicationRole>()
      .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>
      (
          identityDbSettings?.ConnectionString, identityDbSettings?.Name
      )
      .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider);
    #endregion

    #region Seed Application User and Roles data
    // TODO: Seed Application User and Roles data
    #endregion

    ConfigureAuthenticationAndAuthorization(services, configuration);

    return services;
  }

  public static IServiceCollection ConfigureAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
  {
    services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

    services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(options =>
      {
        var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()!;

        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = jwtSettings.Issuer,
          ValidAudience = jwtSettings.Audience,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
      });

    return services;
  }
}
