using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using System.Collections.Concurrent;
using System.Diagnostics;

namespace CZGL.ProcessMetrics.Counters
{
    /// <summary>
    /// 监听 CLR 中的所有事件
    /// </summary>
    public sealed class NETEventSourceListener : EventListener, IMerticsSource
    {
        protected override void OnEventSourceCreated(EventSource source)
        {
            /*
             * CLR 中包含以下监视器
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

            if (MetricsOption.Listeners
                .FirstOrDefault(x => x.Equals(source.Name, StringComparison.OrdinalIgnoreCase)) == null)
                return;

            EnableEvents(source, EventLevel.Verbose, EventKeywords.All, new Dictionary<string, string>()
            {
                ["EventCounterIntervalSec"] = "1"
            });
        }

        private static readonly ConcurrentDictionary<string, IDictionary<string, object>> ListenerPayload;

        private static DateTime lastDatetime = DateTime.Now;

        static NETEventSourceListener()
        {
            ListenerPayload = new ConcurrentDictionary<string, IDictionary<string, object>>();
        }

        // 事件源写入处，由 CLR 自动调用
        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            if (eventData.Payload.Count == 0)
                return;

            // 500ms 内只能写入一次，避免数据刷入过于频繁
            if (DateTime.Now - lastDatetime < TimeSpan.FromMilliseconds(500))
                return;

            try
            {
                foreach (var item in eventData.Payload)
                {
                    if (item == null) continue;
                    if (item is IDictionary<string, object> eventPayload)
                    {
                        if (!ListenerPayload.ContainsKey(eventPayload["Name"].ToString()))
                            _ = ListenerPayload.TryAdd(eventPayload["Name"].ToString(), null);
                        ListenerPayload[eventPayload["Name"].ToString()] = eventPayload;
                    }

                }
                lastDatetime = DateTime.Now;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
            }
        }

        public async Task InvokeAsync(ProcessMetricsCore metricsCore)
        {
            if (ListenerPayload == null)
            {
                await Task.CompletedTask;
                return;
            }

            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

            await Task.Factory.StartNew(() =>
            {
                try
                {

                    foreach (var item in ListenerPayload.Values)
                    {
                        GetRelevantMetric(item, metricsCore);
                    }
                    _ = taskCompletionSource.TrySetResult(true);
                }
                catch
                {
                    _ = taskCompletionSource.TrySetResult(false);
                }
            });
            await taskCompletionSource.Task;
        }


        // 解析其中一个值生成:
        // dotnet_xxx{label="value"} value
        private static void GetRelevantMetric(
            IDictionary<string, object> eventPayload,
            ProcessMetricsCore processMetricsCore)
        {
            if (!eventPayload.TryGetValue("Name", out object name)) return;
            if (!eventPayload.TryGetValue("DisplayName", out object displayValue))
            {
                displayValue = string.Empty;
            }

            var gauge = processMetricsCore
                .CreateGauge("dotnet_clr_" + name.ToString().Replace("-", "_")
                , displayValue.ToString());
            var labels = gauge.Create();

            // 只要能够取得两者之一的值即可
            if (
                eventPayload.TryGetValue("Mean", out var value) ||
                eventPayload.TryGetValue("Increment", out value))
            {
                if (!decimal.TryParse(value.ToString(), out var v))
                {
                    v = 0;
                }

                // 获取计量单位
                if (!eventPayload.TryGetValue("DisplayUnits", out var units))
                {
                    labels.SetValue(v);
                    return;
                }


                var parseValue = ParseUnits(v, units.ToString());
                labels.AddLabel("display_units", parseValue.units);
                labels.SetValue(parseValue.value);
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
