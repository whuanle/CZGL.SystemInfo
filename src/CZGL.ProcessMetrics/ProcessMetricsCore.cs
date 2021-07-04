using CZGL.ProcessMetrics.Counters;
using CZGL.ProcessMetrics.Prometheus;
using CZGL.SystemInfo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CZGL.ProcessMetrics
{
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

        public string GetPrometheus()
        {
            processInfo.Refresh();

            StringBuilder stringBuilder = new StringBuilder();

            ProcessRecord(stringBuilder);

            CpuUsegaeRecord(stringBuilder);

            ProcessThreadsRecord(stringBuilder);

            GCReocrd(stringBuilder);

            DiskRecord(stringBuilder);

            NetworkRecord(stringBuilder);

            CLRRecord(stringBuilder);

            foreach (var item in prometheusFormats)
            {
                stringBuilder.Append(item.BuildMetrice());
            }
            stringBuilder.Append("\n");

            prometheusFormats.Clear();
            return stringBuilder.ToString();
        }

        private void NetworkRecord(StringBuilder stringBuilder)
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
                    .AddValue((speed.Sent.OriginSize >> 10));

                networksreceivedLabel.AddLabel("name", item.Name)
                    .AddLabel("ip_address", item.AddressIpv4.ToString())
                    .AddLabel("mac", item.Mac)
                    .AddLabel("status", item.Status.ToString())
                    .AddLabel("network_interface_type", item.NetworkType.ToString())
                    .AddLabel("received_size_bytes", item.ReceivedLengthIpv4.ToString())
                    .AddLabel("speed_received", (speed.Received.OriginSize >> 10).ToString())
                    .AddValue((speed.Received.OriginSize >> 10));

            }
        }

        private void DiskRecord(StringBuilder stringBuilder)
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
                    .AddValue(item.TotalSize);
            }
        }

        private void GCReocrd(StringBuilder stringBuilder)
        {
            var collectionCountsParent = CreateCounter("dotnet_collection_count_total", "the number of times garbage collection has occurred for the specified generation of objects.", 0);
            for (var gen = 0; gen <= GC.MaxGeneration; gen++)
            {
                var recycleCount = GC.CollectionCount(gen);
                var gcCounterLabels = collectionCountsParent.Create();
                gcCounterLabels
                    .AddLabel("generation", $"{gen}")
                    .AddValue(recycleCount);
            }


            var gcMemoryGauge = CreateGauge("gc_memory_info", "Gets garbage collection memory information");
            var gcMemoryInfo = GC.GetGCMemoryInfo();
            gcMemoryGauge.Create()
                .AddLabel("fragmented_bytes", gcMemoryInfo.FragmentedBytes.ToString())
                .AddLabel("heap_size_bytes", gcMemoryInfo.HeapSizeBytes.ToString())
                .AddLabel("high_memory_load_lhreshold_bytes", gcMemoryInfo.HighMemoryLoadThresholdBytes.ToString())
                .AddLabel("memory_load_bytes", gcMemoryInfo.MemoryLoadBytes.ToString())
                .AddLabel("total_available_memory_bytes", gcMemoryInfo.TotalAvailableMemoryBytes.ToString())
                .AddValue(0);


            var totalAllocatedBytesGauge = CreateGauge("total_allocated_bytes", "Gets a count of the bytes allocated over the lifetime of the process.");
            var totalAllocatedLabels = totalAllocatedBytesGauge.Create();
            totalAllocatedLabels.AddValue(GC.GetTotalAllocatedBytes());
        }

        private void ProcessThreadsRecord(StringBuilder stringBuilder)
        {
            var openHandles = CreateGauge("process_open_handles", "Number of open handles");
            openHandles.Create()
                .AddValue(processInfo.Process.HandleCount);

            var numThreads = CreateGauge("process_num_threads", "Total number of threads");
            numThreads.Create()
                .AddValue(processInfo.Process.Threads.Count);
        }

        private void CpuUsegaeRecord(StringBuilder stringBuilder)
        {
            Gauge processCpuage = CreateGauge("process_cpu_age", "cpuage");
            processCpuage.Create()
                .AddLabel("process_name", processInfo.MainModule)
                .AddValue(ProcessInfo.GetCpuUsage(processInfo));
        }

        private void ProcessRecord(StringBuilder stringBuilder)
        {
            Gauge processMemory = CreateGauge("process_physical_use_memory_bytes", "Memory already used by the current program");
            processMemory.Create()
                .AddLabel("process_name", processInfo.MainModule)
                .AddValue(processInfo.PhysicalUsedMemory);
        }

        private void CLRRecord(StringBuilder stringBuilder)
        {
            Gauge monitor = CreateGauge("dotnet_lock_contention_total", "Provides a mechanism that synchronizes access to objects.");
            monitor.Create()
                .AddValue(Monitor.LockContentionCount);
        }
    }
}
