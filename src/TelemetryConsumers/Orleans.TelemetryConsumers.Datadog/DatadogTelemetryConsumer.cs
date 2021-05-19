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
        private readonly string[] _metricPrefixes;

        public DatadogTelemetryConsumer(IOptions<DatadogTelemetryConsumerOptions> options)
        {
            var config = options.Value.StatsDConfig ?? throw new ArgumentException();
            _metricPrefixes = options.Value?.MetricPrefixes ?? throw new ArgumentException();
            _service = new DogStatsdService();
            _service.Configure(config);
        }

        /// <inheritdoc />
        public void DecrementMetric(string name)
        {
            if (ShouldRecord(name))
            {
                this._service.Decrement(FormatMetricName(name));
            }
        }

        /// <inheritdoc />
        public void DecrementMetric(string name, double value)
        {
            if (ShouldRecord(name))
            {
                this._service.Decrement(FormatMetricName(name), (int)Math.Clamp(value, int.MinValue, int.MaxValue));
            }
        }

        /// <inheritdoc />
        public void IncrementMetric(string name)
        {
            if (ShouldRecord(name))
            {
                this._service.Increment(FormatMetricName(name));
            }
        }

        /// <inheritdoc />
        public void IncrementMetric(string name, double value)
        {
            if (ShouldRecord(name))
            {
                this._service.Increment(FormatMetricName(name), (int)Math.Clamp(value, int.MinValue, int.MaxValue));
            }
        }

        /// <inheritdoc />
        public void TrackMetric(string name, TimeSpan value, IDictionary<string, string> properties = null)
        {
            if (ShouldRecord(name))
            {
                this._service.Distribution(FormatMetricName(name), value.TotalMilliseconds, tags: GetTags(properties));
            }
        }

        /// <inheritdoc />
        public void TrackMetric(string name, double value, IDictionary<string, string> properties = null)
        {
            if (ShouldRecord(name))
            {
                this._service.Distribution(FormatMetricName(name), value, tags: GetTags(properties));
            }
        }

        /// <inheritdoc />
        public void Flush() => _service.Flush();

        /// <inheritdoc />
        public void Close() => _service.Flush();

        /// <inheritdoc />
        public void Dispose() => _service.Dispose();

        private bool ShouldRecord(string name) =>
            _metricPrefixes.Any(prefix => name.StartsWith(prefix));

        private static string FormatMetricName(string name) =>
            $"orleans.{name}";
        
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
    }
}