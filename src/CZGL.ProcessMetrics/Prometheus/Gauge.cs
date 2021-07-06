using System;
using System.Collections.Generic;
using System.Text;

namespace CZGL.ProcessMetrics.Prometheus
{
    /// <summary>
    /// 数据计量器
    /// </summary>
    public class Gauge : PrometheusFormat
    {
        public Gauge(string name, string describetion) : base(MetriceType.Gauge, name, describetion)
        {
        }
    }
}
