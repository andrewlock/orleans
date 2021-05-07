using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using StatsdClient;

namespace Orleans.TelemetryConsumers.Datadog
{
    public class DatadogTelemetryConsumer : IMetricTelemetryConsumer, IDisposable
    {
        private readonly DogStatsdService _service;
        private readonly Func<string, string> _formatMetricName;
        public DatadogTelemetryConsumer(IOptions<DatadogTelemetryConsumerOptions> options)
        {
            var config = options.Value.StatsDConfig ?? throw new ArgumentException();
            _formatMetricName = options.Value.MetricFormatter ?? throw new ArgumentException();
            _service = new DogStatsdService();
            _service.Configure(config);
        }

        public void DecrementMetric(string name) =>
            this._service.Decrement(_formatMetricName(name));

        public void DecrementMetric(string name, double value) =>
            this._service.Decrement(_formatMetricName(name), (int)Math.Clamp(value, int.MinValue, int.MaxValue));

        public void IncrementMetric(string name)=>
            this._service.Increment(_formatMetricName(name));

        public void IncrementMetric(string name, double value) =>
            this._service.Increment(_formatMetricName(name), (int)Math.Clamp(value, int.MinValue, int.MaxValue));

        public void TrackMetric(string name, TimeSpan value, IDictionary<string, string> properties = null) =>
            this._service.Timer(_formatMetricName(name), value.TotalMilliseconds, tags: GetTags(properties));

        public void TrackMetric(string name, double value, IDictionary<string, string> properties = null) =>
            this._service.Timer(_formatMetricName(name), value, tags: GetTags(properties));

        private static string[] GetTags(IDictionary<string, string> properties)
        {
            if (properties is null)
            {
                return null;
            }

            return properties
                .Select(kvp => $"{kvp.Key.Replace(':', '_')}:{kvp.Value.Replace(':', '_')}")
                .ToArray();
        }

        public void Flush() => _service.Flush();

        public void Close() => _service.Flush();

        public void Dispose() => _service.Dispose();
    }
}