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
}
