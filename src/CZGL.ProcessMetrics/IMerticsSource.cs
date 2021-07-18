using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CZGL.ProcessMetrics
{
    /// <summary>
    /// Metrics 源
    /// </summary>
    public interface IMerticsSource
    {
        /// <summary>
        /// 生成 Metrics 数据
        /// </summary>
        /// <param name="metricsCore"></param>
        /// <returns></returns>
        Task InvokeAsync(ProcessMetricsCore metricsCore);
    }
}
