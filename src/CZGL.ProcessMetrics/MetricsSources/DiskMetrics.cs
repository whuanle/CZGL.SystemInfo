using CZGL.ProcessMetrics.Prometheus;
using CZGL.SystemInfo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CZGL.ProcessMetrics.MetricsSources
{
    public class DiskMetrics : IMerticsSource
    {
        private readonly DiskInfo[] diskInfos;
        public DiskMetrics()
        {
            diskInfos = DiskInfo.GetRealDisk();
        }
        public async Task InvokeAsync(ProcessMetricsCore metricsCore)
        {
            await Task.Factory.StartNew(() =>
            {
                Gauge disks = metricsCore.CreateGauge("dotnet_drives_info", "all drives info");

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
    }
}
