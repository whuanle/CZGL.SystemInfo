#if NETCOREAPP3_0 || NETCOREAPP3_1

using CZGL.ProcessMetrics.Prometheus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CZGL.ProcessMetrics.Prometheus;
using System.Threading;

namespace CZGL.ProcessMetrics.MetricsSources
{
    public class CLRMetrics : IMerticsSource
    {
        public async Task InvokeAsync(ProcessMetricsCore metricsCore)
        {
            await Task.Factory.StartNew(() =>
            {

                Gauge monitor = metricsCore.CreateGauge("dotnet_lock_contention_total", "Provides a mechanism that synchronizes access to objects.");
                monitor.Create()
                    .SetValue(Monitor.LockContentionCount);
            });
        }
    }
}
#endif