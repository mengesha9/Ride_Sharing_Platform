using Microsoft.OpenApi.Models;
using Rideshare.Application;
using Rideshare.Infrastructure;
using Rideshare.Infrastructure.Persistence;
using Swashbuckle.AspNetCore.SwaggerUI;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using System.Text.Json.Serialization;
using Rideshare.WebApi.Services;
using Rideshare.WebApi.Middleware;
using Rideshare.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Dependency Injection configurations for the projects
await builder.Services
    .AddApplication()
    .AddIdentity(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddPersistenceAsync(builder.Configuration);

// Configure Kestrel to listen on port 8080
builder.WebHost.UseUrls("http://*:8080");
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(MongoDB.Bson.BsonType.String));

// Add services to the container.
builder.Services.AddScoped<MongoDbContext>();
builder.Services.AddControllers().AddJsonOptions(opts =>
{
    var enumConverter = new JsonStringEnumConverter();
    opts.JsonSerializerOptions.Converters.Add(enumConverter);
});

builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();

// Add CORS services configuration to allow all origins, headers, and methods
builder.Services.AddCors(options =>
{
    options.AddPolicy("OpenCorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Swagger configuration
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rideshare API", Version = "v1" });

    // Define the OAuth2.0 scheme that's being used for the API
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            Array.Empty<string>()
        }
    });
});

// DI for the WebApi layer
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUserAccessor, UserAccessor>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RiderPolicy", policy =>
    {

        policy.RequireAuthenticatedUser();
        policy.RequireRole("Rider");

    });

    options.AddPolicy("AdminPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Admin");
    });
});

// Chapa configuration
string? API_SECRET = builder.Configuration["Chapa:TestSecretkey"];
Console.WriteLine(API_SECRET);

builder.Services.AddScoped<MongoDbContext, MongoDbContext>();
// Register VehiclePricesOptions configuration
builder.Services.Configure<VehiclePricesOptions>(builder.Configuration.GetSection("VehiclePricesPerKm"));

// Register PaymentCalculatorService
builder.Services.AddTransient<PaymentCalculatorService>();

var app = builder.Build();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
// specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rideshare API V1");
    c.RoutePrefix = "swagger"; // This will set the swagger UI route to 'http://localhost:8080/swagger'
    c.DocExpansion(DocExpansion.None);
});

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();

// Use the CORS policy
app.UseCors("OpenCorsPolicy");


// Map controllers
app.MapControllers();

app.Run();