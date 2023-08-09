using Common.Settings;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Common.MassTransit
{
    public static class Extenstions
    {
        public static void MassTransite(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(configure =>
            {
                configure.AddConsumers(Assembly.GetEntryAssembly());
                configure.UsingRabbitMq((context, configurator) =>
                {
                    var rabbitMQSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
                    var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                    configurator.Host(rabbitMQSettings?.Host);
                    configurator.ConfigureEndpoints
                        (context, new KebabCaseEndpointNameFormatter(serviceSettings?.ServiceName, false));
                });
            });

        }
    }
}
