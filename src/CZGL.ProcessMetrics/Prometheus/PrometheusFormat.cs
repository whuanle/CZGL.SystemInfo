using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZGL.ProcessMetrics.Prometheus
{
    public abstract class PrometheusFormat
    {

        protected readonly MetriceType _metriceType;
        protected readonly string _metricName;
        protected readonly string _describetion;

        protected readonly List<LabelValue> labelValues;

        public string MetriceName => _metricName;
        public string Describetion => _describetion;


        protected PrometheusFormat(MetriceType metriceType, string metricName, string describetion)
        {
            (_metriceType, _metricName, _describetion) = (metriceType, metricName.ToLower(), describetion);
            labelValues = new List<LabelValue>();
        }


        public ILabel Create()
        {
            var label = new LabelValue();
            labelValues.Add(label);
            return label;
        }

        public virtual string BuildMetrice()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"# HELP {_metricName}  {_describetion} .");
            stringBuilder.AppendLine($"# TYPE {_metricName} {Enum.GetName(typeof(MetriceType), _metriceType).ToLower()}");

            foreach (var item in labelValues)
            {
                stringBuilder.AppendLine($"{_metricName} {item.BuildMetrice()}");
            }

            return stringBuilder.ToString().Replace("\r\n","\n");
        }


    }
}
