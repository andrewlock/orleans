using System;
using StatsdClient;

namespace Orleans.TelemetryConsumers.Datadog
{
    public class DatadogTelemetryConsumerOptions
    {
        public StatsdConfig StatsDConfig { get; set; } = new StatsdConfig();

        /// <summary>
        /// Only metrics which start with one of these values will be recorded with DogStatsD
        /// </summary>
        public string[] MetricPrefixes { get; set; } = {
            "App.Requests.",
            "Grain."
        };
    }
}