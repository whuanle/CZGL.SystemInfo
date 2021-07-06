using System;
using System.Collections.Generic;
using System.Text;

namespace CZGL.ProcessMetrics.Prometheus
{
    /// <summary>
    /// 当前指标记录器类型。
    /// 目前只使用了 Counter、Gauge
    /// </summary>
    public enum MetriceType
    {
        Counter,
        Gauge,
        Summary
    }
}
