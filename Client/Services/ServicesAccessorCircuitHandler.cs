using Microsoft.AspNetCore.Components.Server.Circuits;

namespace Client.Services
{
    public class ServicesAccessorCircuitHandler(
    IServiceProvider services, CircuitServicesAccessor servicesAccessor)
    : CircuitHandler
    {
        public override Func<CircuitInboundActivityContext, Task> CreateInboundActivityHandler(
            Func<CircuitInboundActivityContext, Task> next) =>
                async context =>
                {
                    servicesAccessor.Services = services;
                    await next(context);
                    servicesAccessor.Services = null;
                };
    }

    public static class CircuitServicesServiceCollectionExtensions
    {
        public static IServiceCollection AddCircuitServicesAccessor(
            this IServiceCollection services)
        {
            services.AddScoped<CircuitServicesAccessor>();
            services.AddScoped<CircuitHandler, ServicesAccessorCircuitHandler>();

            return services;
        }
    }
}
