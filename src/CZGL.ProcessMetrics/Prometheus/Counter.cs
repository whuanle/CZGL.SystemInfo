using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CZGL.ProcessMetrics.Prometheus
{
    /// <summary>
    /// 计数器
    /// </summary>
    public class Counter : PrometheusFormat
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">监控名称</param>
        /// <param name="describetion">描述值</param>
        /// <param name="value">初始化值</param>
        internal Counter(string name, string describetion, decimal value, MetricsOption option)
            : base(MetriceType.Counter, name, describetion, option)
        {
        }
    }
}
