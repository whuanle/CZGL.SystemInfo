using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CZGL.ProcessMetrics.Prometheus
{
    public class Counter : PrometheusFormat
    {
        public Counter(string name, string describetion,decimal value) : base(MetriceType.Counter, name, describetion)
        {
        }
    }
}
