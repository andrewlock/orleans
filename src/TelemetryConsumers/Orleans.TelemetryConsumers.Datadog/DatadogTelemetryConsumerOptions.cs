using System;
using StatsdClient;

namespace Orleans.TelemetryConsumers.Datadog
{
    public class DatadogTelemetryConsumerOptions
    {
        public StatsdConfig StatsDConfig { get; set; } = new StatsdConfig();

        public Func<string, string> MetricFormatter { get; set; } = x => x;
    }
}