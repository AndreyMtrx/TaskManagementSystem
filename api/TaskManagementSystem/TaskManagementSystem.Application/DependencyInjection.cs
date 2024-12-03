using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Reflection;
using TaskManagementSystem.Application.Configuration;

namespace TaskManagementSystem.Application;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<ServiceBusConfig>(builder.Configuration.GetSection("ServiceBus"));

        builder.Services.AddSingleton<IServiceBusHandler>(provider =>
        {
            var config = provider.GetRequiredService<IOptions<ServiceBusConfig>>().Value;
            return new ServiceBusHandler(config.ConnectionString, config.QueueName);
        });


        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

        builder.Services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
    }
}
