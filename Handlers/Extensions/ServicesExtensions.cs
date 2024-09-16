using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Pretendo.Backend.Handlers.Extensions
{
    /// <summary>
    /// Registers any implementation of 'IHandler' into the DI
    /// </summary>
    public static class ServicesExtensions
    {
        /// <summary>
        /// Uses reflection to find all implementations of 'IHandler' and register them as a service in DI
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IServiceCollection AddHandlers(this IServiceCollection services, Assembly assembly)
        {
            ServiceDescriptor[] serviceDescriptors = assembly
                .DefinedTypes
                .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                        type.IsAssignableTo(typeof(IHandler)))
                .Select(type => ServiceDescriptor.Transient(typeof(IHandler), type))
                .ToArray();
            services.TryAddEnumerable(serviceDescriptors);
            return services;
        }
    }
}
