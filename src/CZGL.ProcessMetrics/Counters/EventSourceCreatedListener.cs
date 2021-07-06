using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace CZGL.ProcessMetrics.Counters
{
    /// <summary>
    /// 监听 CLR 中的所有事件
    /// </summary>
    sealed class EventSourceCreatedListener : EventListener
    {
        protected override void OnEventSourceCreated(EventSource source)
        {
            /*
             * CLR 中包含以下监视器，但是这里只使用 System.Runtime
             * Microsoft-Windows-DotNETRuntime
             * System.Runtime
             * Microsoft-System-Net-Http
             * System.Diagnostics.Eventing.FrameworkEventSource
             * Microsoft-Diagnostics-DiagnosticSource
             * Microsoft-System-Net-Sockets
             * Microsoft-System-Net-NameResolution
             * System.Threading.Tasks.TplEventSource
             * System.Buffers.ArrayPoolEventSource
             * Microsoft-System-Net-Security
             * System.Collections.Concurrent.ConcurrentCollectionsEventSource
             */
            if (!source.Name.Equals("System.Runtime"))
                return;

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

        // 解析其中一个值生成:
        // dotnet_xxx{label="value"} value
        private static void GetRelevantMetric(
            IDictionary<string, object> eventPayload)
        {
            if (!eventPayload.TryGetValue("Name", out object name)) return;
            if (!eventPayload.TryGetValue("DisplayName", out object displayValue))
            {
                displayValue = string.Empty;
            }

            var processMetrics = ProcessMetricsCore.Instance;
            var gauge = processMetrics
                .CreateGauge("dotnet_" + name.ToString().Replace("-", "_")
                , displayValue.ToString());
            var labels = gauge.Create();

            // 只要能够取得两者之一的值即可
            if (eventPayload.TryGetValue("Mean", out object value) ||
                eventPayload.TryGetValue("Increment", out value))
            {
                if (!decimal.TryParse(value.ToString(), out var v))
                {
                    v = 0;
                }

                // 获取计量单位
                if (!eventPayload.TryGetValue("DisplayUnits", out var units))
                {
                    labels.AddValue(v);
                    return;
                }


                var parseValue = ParseUnits(v, units.ToString());
                labels.AddLabel("display_units", parseValue.units);
                labels.AddValue(parseValue.value);
            }
        }

        /// <summary>
        /// 转换计量单位。不会损失精确度
        /// 例如 72.6MB，变成 72.6*1024*1024=76,126,617.6 字节，在 Grafana 中最终也会换成 MB
        /// </summary>
        /// <param name="objectValue"></param>
        /// <param name="units"></param>
        /// <returns></returns>
        private static (decimal value, string units) ParseUnits(object objectValue, string units)
        {
            if (!long.TryParse(objectValue.ToString(), out var v)) return (0, string.Empty);

            if (units == null || units == string.Empty)
                return (v, units);

            if (units.Equals("KB", StringComparison.OrdinalIgnoreCase))
                return ((v << 10), "B");

            if (units.Equals("MB", StringComparison.OrdinalIgnoreCase))
                return ((v << 20), "B");

            if (units.Equals("GB", StringComparison.OrdinalIgnoreCase))
                return ((v << 30), "B");

            return (v, units);
        }
    }

}
