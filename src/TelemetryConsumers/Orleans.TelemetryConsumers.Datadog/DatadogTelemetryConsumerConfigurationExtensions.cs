using Microsoft.Extensions.DependencyInjection;
using Orleans.Configuration;
using Orleans.TelemetryConsumers.Datadog;

namespace Orleans.Hosting
{
    public static class DatadogTelemetryConsumerConfigurationExtensions
    {
        /// <summary>
        /// Adds a metrics telemetric consumer provider of type <see cref="DatadogTelemetryConsumer"/>.
        /// </summary>
        /// <param name="hostBuilder"></param>
        public static ISiloHostBuilder AddDatadogTelemetryConsumer(this ISiloHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices((context, services) => ConfigureServices(context, services));
        }

        /// <summary>
        /// Adds a metrics telemetric consumer provider of type <see cref="DatadogTelemetryConsumer"/>.
        /// </summary>
        /// <param name="hostBuilder"></param>
        public static ISiloBuilder AddDatadogTelemetryConsumer(this ISiloBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices((context, services) => ConfigureServices(context, services));
        }

        /// <summary>
        /// Adds a metrics telemetric consumer provider of type <see cref="DatadogTelemetryConsumer"/>.
        /// </summary>
        /// <param name="clientBuilder"></param>
        public static IClientBuilder AddDatadogTelemetryConsumer(this IClientBuilder clientBuilder)
        {
            return clientBuilder.ConfigureServices((context, services) => ConfigureServices(context, services));
        }

        private static void ConfigureServices(Microsoft.Extensions.Hosting.HostBuilderContext context, IServiceCollection services)
        {
            services.Configure<TelemetryOptions>(options => options.AddConsumer<DatadogTelemetryConsumer>());
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.Configure<TelemetryOptions>(options => options.AddConsumer<DatadogTelemetryConsumer>());
        }

    }
}
