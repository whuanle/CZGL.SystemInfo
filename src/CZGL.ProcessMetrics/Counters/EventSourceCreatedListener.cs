using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace CZGL.ProcessMetrics.Counters
{
    sealed class EventSourceCreatedListener : EventListener
    {
        protected override void OnEventSourceCreated(EventSource source)
        {
            EnableEvents(source, EventLevel.Verbose, EventKeywords.All, new Dictionary<string, string>()
            {
                ["EventCounterIntervalSec"] = "1"
            });
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            for (int i = 0; i < eventData.Payload.Count; ++i)
            {
                if (eventData.Payload[i] is IDictionary<string, object> eventPayload)
                {
                    GetRelevantMetric(eventPayload);
                }
            }
        }

        private static void GetRelevantMetric(
            IDictionary<string, object> eventPayload)
        {
            if (!eventPayload.TryGetValue("Name", out object name)) return;
            if (eventPayload.TryGetValue("DisplayName", out object displayValue)) { }

            var processMetrics = ProcessMetricsCore.Instance;
            var gauge = processMetrics.CreateGauge("dotnet_" + name.ToString().Replace("-", "_"), displayValue.ToString());
            var labels = gauge.Create();

            if (eventPayload.TryGetValue("Mean", out object value) ||
                eventPayload.TryGetValue("Increment", out value))
            {
                var v = decimal.Parse(value.ToString());

                if (eventPayload.TryGetValue("DisplayUnits", out var units)) { };


                var parseValue = Parse(v,units.ToString());
                labels.AddLabel("display_units", parseValue.units);
                labels.AddValue(parseValue.value);
            }
        }

        private static (decimal value, string units) Parse(object value, string units)
        {
            if (!int.TryParse(value.ToString(), out var v)) return (0, "");

            if (units == null)
                return (decimal.Parse(value.ToString()), units);

            if (units.Equals("KB", StringComparison.OrdinalIgnoreCase))
                return ((decimal)(v << 10), "B");

            if (units.Equals("MB", StringComparison.OrdinalIgnoreCase))
                return ((decimal)(v << 20), "B");

            if (units.Equals("GB", StringComparison.OrdinalIgnoreCase))
                return ((decimal)(v << 30), "B");

            return (decimal.Parse(value.ToString()), units);
        }
    }

}
