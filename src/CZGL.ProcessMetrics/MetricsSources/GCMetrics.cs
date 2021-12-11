using CZGL.SystemInfo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace CZGL.ProcessMetrics.MetricsSources
{
    /// <summary>
    /// GC 信息
    /// </summary>
    public class GCMetrics : IMerticsSource
    {
        public async Task InvokeAsync(ProcessMetricsCore metricsCore)
        {
            await Task.Factory.StartNew(() =>
            {
                var collectionCountsParent = metricsCore.CreateCounter("dotnet_collection_count_total", "the number of times garbage collection has occurred for the specified generation of objects.", 0);
                var hour = Math.Round((decimal)(DateTime.Now - Process.GetCurrentProcess().StartTime).TotalHours, 1);

                for (var gen = 0; gen <= GC.MaxGeneration; gen++)
                {
                    var recycleCount = GC.CollectionCount(gen);
                    var gcCounterLabels = collectionCountsParent.Create();
                    gcCounterLabels
                        .AddLabel("generation", $"{gen}")
                        .AddLabel("run_time_hour", hour.ToString())
                        .SetValue(recycleCount);
                }


                var gcMemoryGauge = metricsCore.CreateGauge("dotnet_gc_memory_info", "Gets garbage collection memory information");
                var gcMemoryInfo = GC.GetGCMemoryInfo();
                gcMemoryGauge.Create()
                    .AddLabel("fragmented_bytes", gcMemoryInfo.FragmentedBytes.ToString())
                    .AddLabel("heap_size_bytes", gcMemoryInfo.HeapSizeBytes.ToString())
                    .AddLabel("high_memory_load_lhreshold_bytes", gcMemoryInfo.HighMemoryLoadThresholdBytes.ToString())
                    .AddLabel("memory_load_bytes", gcMemoryInfo.MemoryLoadBytes.ToString())
                    .AddLabel("total_available_memory_bytes", gcMemoryInfo.TotalAvailableMemoryBytes.ToString())
                    .SetValue(0);

                var fragmented = metricsCore.CreateGauge("dotnet_gc_fragmented_bytes", "Gets garbage collection memory information");
                fragmented.Create()
                .SetValue((decimal)gcMemoryInfo.FragmentedBytes);

                var heap = metricsCore.CreateGauge("dotnet_gc_heap_size_bytes", "Gets garbage collection memory information");
                heap.Create()
                .SetValue((decimal)gcMemoryInfo.HeapSizeBytes);

                var highLoad = metricsCore.CreateGauge("dotnet_gc_high_memory_load_lhreshold_bytes", "Gets garbage collection memory information");
                highLoad.Create()
                .SetValue((decimal)gcMemoryInfo.HighMemoryLoadThresholdBytes);

                var load = metricsCore.CreateGauge("dotnet_gc_memory_load_bytes", "Gets garbage collection memory information");
                load.Create()
                .SetValue((decimal)gcMemoryInfo.MemoryLoadBytes);

                var available = metricsCore.CreateGauge("dotnet_gc_total_available_memory_bytes", "Gets garbage collection memory information");
                available.Create()
                .SetValue((decimal)gcMemoryInfo.TotalAvailableMemoryBytes);


                var totalAllocatedBytesGauge = metricsCore.CreateGauge("dotnet_total_allocated_bytes", "Gets a count of the bytes allocated over the lifetime of the process.");
                var totalAllocatedLabels = totalAllocatedBytesGauge.Create();
                totalAllocatedLabels.SetValue(GC.GetTotalAllocatedBytes());
            });
        }

    }
}
