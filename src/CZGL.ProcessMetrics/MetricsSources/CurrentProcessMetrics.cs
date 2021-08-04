using CZGL.ProcessMetrics.Prometheus;
using CZGL.SystemInfo;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CZGL.ProcessMetrics.MetricsSources
{
    /// <summary>
    /// 当前进程的 Metrics
    /// </summary>
    public class CurrentProcessMetrics : IMerticsSource
    {
        private static readonly ProcessInfo processInfo;
        static CurrentProcessMetrics()
        {
            processInfo = ProcessInfo.GetCurrentProcess();
        }

        private ProcessMetricsCore _metricsCore;
        public async Task InvokeAsync(ProcessMetricsCore metricsCore)
        {
            processInfo.Refresh();
            _metricsCore = metricsCore;
            List<Task> tasks = new List<Task>()
            {
                ProcessThreads(),
                CpuUsage(),
                Memory(),
                Info()
            };
            await Task.WhenAll(tasks);
        }

        // 句柄和线程数量
        private async Task ProcessThreads()
        {
            await Task.Factory.StartNew(() =>
            {
                // 句柄（handle）是Windows操作系统用来标识被应用程序所创建或使用的对象的整数。
                var openHandles = _metricsCore.CreateGauge("dotnet_process_open_handles_count",
                    "Common resource handles includefile descriptors,network sockets,database connections,process identifiers(PIDs), andjob IDs");
                openHandles.Create()
                .AddLabel("metrics_type", "procsss")
                .SetValue(processInfo.Process.HandleCount);

                // 可用辅助线程的数目
                // 可用异步 I/O 线程的数目
                ThreadPool.GetAvailableThreads(out var workerCount,out var comCount);
                var worker = _metricsCore.CreateGauge("dotnet_threadpool_worker_count",
    "Retrieves the difference between the maximum number of thread pool threads returned by the GetMaxThreads(Int32, Int32) method, and the number currently active.");
                worker.Create()
                .SetValue(workerCount);
                var completion = _metricsCore.CreateGauge("dotnet_threadpool_completion_count",
"Retrieves the difference between the maximum number of thread pool threads returned by the GetMaxThreads(Int32, Int32) method, and the number currently active.");
                completion.Create()
                .SetValue(comCount);


#if NETCOREAPP3_0 || NETCOREAPP3_1
                // 获取当前已加入处理队列的工作项数
                var pendingWorkItemCount = _metricsCore.CreateGauge("dotnet_threadpool_pendingworkitem_count",
"Gets the number of work items that are currently queued to be processed.");
                pendingWorkItemCount.Create()
                .SetValue((int)ThreadPool.PendingWorkItemCount);

                // 获取当前存在的线程池线程数。
                var threadPoolCount = _metricsCore.CreateGauge("dotnet_threadpool_count",
"Gets the number of thread pool threads that currently exist.");
                threadPoolCount.Create()
                .SetValue((int)ThreadPool.ThreadCount);
#endif

                var numThreads = _metricsCore.CreateGauge("dotnet_process_num_threads_count", "Total number of threads");
                numThreads.Create()
                .AddLabel("metrics_type", "procsss")
                .SetValue(processInfo.Process.Threads.Count);
            });
        }

        /// <summary>
        /// CPU 使用率
        /// </summary>
        /// <returns></returns>
        private async Task CpuUsage()
        {
            await Task.Factory.StartNew(() =>
            {
                Gauge processCpuage = _metricsCore.CreateGauge("dotnet_process_cpu_usage", "This process cpu usage");
                processCpuage.Create()
                    .SetValue(ProcessInfo.GetCpuUsage(processInfo));
            });
        }

        // 内存
        private async Task Memory()
        {
            await Task.Factory.StartNew(() =>
            {
                Gauge processMemory = _metricsCore.CreateGauge("dotnet_process_physical_used_memory_bytes", "Memory already used by the current program");
                processMemory.Create()
                    .SetValue(processInfo.PhysicalUsedMemory);
            });
        }

        // 进程信息
        private async Task Info()
        {
            await Task.Factory.StartNew(() =>
            {
                Gauge processMemory = _metricsCore.CreateGauge("dotnet_process_info", "Process info");
                var labels = processMemory.Create()
                    .AddLabel("id", processInfo.ProcessId.ToString())
                    .AddLabel("name", processInfo.ProcessName)
                    .AddLabel("start_time", processInfo.StartTime.ToString("yyyy/MM/dd HH:mm:ss"))
                    .AddLabel("tick_run_time_ms", Environment.TickCount.ToString())
                    .AddLabel("physical_used_memory_bytes", processInfo.PhysicalUsedMemory.ToString());
                try
                {
                    labels.AddLabel("base_priority", processInfo.BasePriority.ToString());
                }
                catch { }
                labels.SetValue(0);

            });
        }
    }
}
