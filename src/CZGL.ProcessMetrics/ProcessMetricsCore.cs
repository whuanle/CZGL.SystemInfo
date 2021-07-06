using CZGL.ProcessMetrics.Counters;
using CZGL.ProcessMetrics.Prometheus;
using CZGL.SystemInfo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CZGL.ProcessMetrics
{
    /// <summary>
    /// 
    /// </summary>
    public class ProcessMetricsCore
    {
        private static readonly ProcessMetricsCore _Instance;
        public static ProcessMetricsCore Instance => _Instance;

        static ProcessMetricsCore()
        {
            var eventSourceListener = new EventSourceCreatedListener();
            _Instance = new ProcessMetricsCore();
        }
        public ProcessMetricsCore()
        {
            prometheusFormats = new List<PrometheusFormat>();
            processInfo = ProcessInfo.GetCurrentProcess();
            diskInfos = DiskInfo.GetRealDisk();
            networkInfos = NetworkInfo.GetRealNetworkInfos();
        }


        private readonly ProcessInfo processInfo;
        private readonly DiskInfo[] diskInfos;
        private readonly NetworkInfo[] networkInfos;
        private readonly List<PrometheusFormat> prometheusFormats;

        public Counter CreateCounter(string name, string describetion, decimal value)
        {
            var gauge = new Counter(name, describetion, value);
            prometheusFormats.Add(gauge);
            return gauge;
        }
        public Gauge CreateGauge(string name, string describetion)
        {
            var gauge = new Gauge(name, describetion);
            prometheusFormats.Add(gauge);
            return gauge;
        }

        /// <summary>
        /// 获取监控信息
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetPrometheus()
        {
            processInfo.Refresh();

            List<Task> metricsTasks = new List<Task>();


            metricsTasks.Add(EventSourceCreatedListener.BuildMetric(_Instance));
            metricsTasks.Add(ProcessRecord());
            metricsTasks.Add(CpuUsegaeRecord());
            metricsTasks.Add(ProcessThreadsRecord());
            metricsTasks.Add(GCReocrd());
            metricsTasks.Add(DiskRecord());
            metricsTasks.Add(NetworkRecord());
            metricsTasks.Add(CLRRecord());

            await Task.WhenAll(metricsTasks);

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in prometheusFormats)
            {
                stringBuilder.Append(item.BuildMetrice());
            }
            stringBuilder.Append("\n");

            prometheusFormats.Clear();
            return stringBuilder.ToString();
        }

        private async Task NetworkRecord()
        {
            await Task.Factory.StartNew(() =>
            {
                Gauge networkinfosGaugeSend = CreateGauge("network_info_send", "network");
                Gauge networkinfosGaugeReceived = CreateGauge("network_info_received", "network");
                foreach (var item in networkInfos)
                {
                    var networksSendLabel = networkinfosGaugeSend.Create();
                    var networksreceivedLabel = networkinfosGaugeReceived.Create();

                    var speed = item.GetInternetSpeed();
                    networksSendLabel.AddLabel("name", item.Name)
                        .AddLabel("ip_address", item.AddressIpv4.ToString())
                        .AddLabel("mac", item.Mac)
                        .AddLabel("status", item.Status.ToString())
                        .AddLabel("network_interface_type", item.NetworkType.ToString())
                        .AddLabel("send_size_bytes", item.SendLengthIpv4.ToString())
                        .AddLabel("speed_send", (speed.Sent.OriginSize >> 10).ToString())
                        .SetValue((speed.Sent.OriginSize >> 10));

                    networksreceivedLabel.AddLabel("name", item.Name)
                        .AddLabel("ip_address", item.AddressIpv4.ToString())
                        .AddLabel("mac", item.Mac)
                        .AddLabel("status", item.Status.ToString())
                        .AddLabel("network_interface_type", item.NetworkType.ToString())
                        .AddLabel("received_size_bytes", item.ReceivedLengthIpv4.ToString())
                        .AddLabel("speed_received", (speed.Received.OriginSize >> 10).ToString())
                        .SetValue((speed.Received.OriginSize >> 10));

                }
            });
        }

        private async Task DiskRecord()
        {
            await Task.Factory.StartNew(() =>
            {
                Gauge disks = CreateGauge("drives_info", "all drives info");

                foreach (var item in diskInfos)
                {
                    var disksLabel = disks.Create();
                    disksLabel.AddLabel("name", item.Name.Replace("\\", "\\\\"))
                        .AddLabel("file_type", item.DriveType.ToString())
                        .AddLabel("file_system", item.FileSystem)
                        .AddLabel("free_size_bytes", item.FreeSpace.ToString())
                        .AddLabel("used_size_bytes", item.UsedSize.ToString())
                        .AddLabel("total", item.TotalSize.ToString())
                        .SetValue(item.TotalSize);
                }
            });
        }

        private async Task GCReocrd()
        {
            await Task.Factory.StartNew(() =>
            {
                var collectionCountsParent = CreateCounter("dotnet_collection_count_total", "the number of times garbage collection has occurred for the specified generation of objects.", 0);
                for (var gen = 0; gen <= GC.MaxGeneration; gen++)
                {
                    var recycleCount = GC.CollectionCount(gen);
                    var gcCounterLabels = collectionCountsParent.Create();
                    gcCounterLabels
                        .AddLabel("generation", $"{gen}")
                        .SetValue(recycleCount);
                }

#if NETCOREAPP3_0 || NETCOREAPP3_1
                var gcMemoryGauge = CreateGauge("gc_memory_info", "Gets garbage collection memory information");
                var gcMemoryInfo = GC.GetGCMemoryInfo();
                gcMemoryGauge.Create()
                    .AddLabel("fragmented_bytes", gcMemoryInfo.FragmentedBytes.ToString())
                    .AddLabel("heap_size_bytes", gcMemoryInfo.HeapSizeBytes.ToString())
                    .AddLabel("high_memory_load_lhreshold_bytes", gcMemoryInfo.HighMemoryLoadThresholdBytes.ToString())
                    .AddLabel("memory_load_bytes", gcMemoryInfo.MemoryLoadBytes.ToString())
                    .AddLabel("total_available_memory_bytes", gcMemoryInfo.TotalAvailableMemoryBytes.ToString())
                    .SetValue(0);


                var totalAllocatedBytesGauge = CreateGauge("total_allocated_bytes", "Gets a count of the bytes allocated over the lifetime of the process.");
                var totalAllocatedLabels = totalAllocatedBytesGauge.Create();
                totalAllocatedLabels.SetValue(GC.GetTotalAllocatedBytes());
#endif
            });
        }

        private async Task ProcessThreadsRecord()
        {
            await Task.Factory.StartNew(() =>
            {
                var openHandles = CreateGauge("process_open_handles", "Number of open handles");
                openHandles.Create()
                    .SetValue(processInfo.Process.HandleCount);

                var numThreads = CreateGauge("process_num_threads", "Total number of threads");
                numThreads.Create()
                    .SetValue(processInfo.Process.Threads.Count);
            });
        }

        private async Task CpuUsegaeRecord()
        {
            await Task.Factory.StartNew(() =>
            {
                Gauge processCpuage = CreateGauge("process_cpu_age", "cpuage");
                processCpuage.Create()
                    .AddLabel("process_name", processInfo.MainModule)
                    .SetValue(ProcessInfo.GetCpuUsage(processInfo));
            });
        }

        private async Task ProcessRecord()
        {
            await Task.Factory.StartNew(() =>
            {
                Gauge processMemory = CreateGauge("process_physical_use_memory_bytes", "Memory already used by the current program");
                processMemory.Create()
                    .AddLabel("process_name", processInfo.MainModule)
                    .SetValue(processInfo.PhysicalUsedMemory);
            });
        }

        private async Task CLRRecord()
        {
            await Task.Factory.StartNew(() =>
            {
#if NETCOREAPP3_0 || NETCOREAPP3_1
                Gauge monitor = CreateGauge("dotnet_lock_contention_total", "Provides a mechanism that synchronizes access to objects.");
                monitor.Create()
                    .SetValue(Monitor.LockContentionCount);
#endif
            });
        }
    }
}
