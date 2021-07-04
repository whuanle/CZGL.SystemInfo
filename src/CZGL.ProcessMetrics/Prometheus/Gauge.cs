using System;
using System.Collections.Generic;
using System.Text;

namespace CZGL.ProcessMetrics.Prometheus
{
    public class Gauge : PrometheusFormat
    {
        public Gauge(string name, string describetion) : base(MetriceType.Gauge, name, describetion)
        {
        }
    }
}
