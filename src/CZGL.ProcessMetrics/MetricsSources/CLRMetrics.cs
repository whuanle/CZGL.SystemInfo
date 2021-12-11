
using CZGL.ProcessMetrics.Prometheus;
using System.Threading;
using System.Threading.Tasks;

namespace CZGL.ProcessMetrics.MetricsSources
{
    /// <summary>
    /// 监控 CLR
    /// </summary>
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