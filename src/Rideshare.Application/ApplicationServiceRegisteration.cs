using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;

namespace Rideshare.Application;
public static class ApplicationServicesRegistration
{
  public static IServiceCollection AddApplication(this IServiceCollection services)
  {
    services.AddAutoMapper(Assembly.GetExecutingAssembly());
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
    return services;
  }
}
