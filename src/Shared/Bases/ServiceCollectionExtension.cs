using Api.Shared.Bases.Attributes;
using System.Reflection;

namespace Api.Shared.Bases;

public static class ServiceCollectionExtension
{
    public static void AddAttributedServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var types = assembly.GetTypes();

        foreach (var type in types)
        {
            var interfaces = type.GetInterfaces();

            if (type.GetCustomAttribute<ScopedServiceAttribute>() != null)
            {
                foreach (var iface in interfaces)
                {
                    services.AddScoped(iface, type);
                }
            }
            else if (type.GetCustomAttribute<SingletonServiceAttribute>() != null)
            {
                foreach (var iface in interfaces)
                {
                    services.AddSingleton(iface, type);
                }
            }
            else if (type.GetCustomAttribute<TransientServiceAttribute>() != null)
            {
                foreach (var iface in interfaces)
                {
                    services.AddTransient(iface, type);
                }
            }
        }
    }
}