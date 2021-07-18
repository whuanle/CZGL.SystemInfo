using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZGL.ProcessMetrics.Prometheus
{
    /// <summary>
    ///  Prometheus 格式
    /// </summary>
    public abstract class PrometheusFormat
    {
        protected readonly MetriceType _metriceType;
        protected readonly string _metricName;
        protected readonly string _describetion;

        protected readonly List<LabelValue> labelValues;
        private readonly MetricsOption _option;

        public string MetriceName => _metricName;
        public string Describetion => _describetion;



        protected PrometheusFormat(MetriceType metriceType, string metricName, string describetion, MetricsOption option)
        {
            (_metriceType, _metricName, _describetion) = (metriceType, metricName.ToLower(), describetion);
            _option = option;
            labelValues = new List<LabelValue>();
        }


        public ILabel Create()
        {
            var label = new LabelValue(_option);
            labelValues.Add(label);
            return label;
        }

        public virtual string BuildMetrice()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"# HELP {_metricName}  {_describetion} .\n");
            stringBuilder.Append($"# TYPE {_metricName} {Enum.GetName(typeof(MetriceType), _metriceType).ToLower()}\n");

            foreach (var item in labelValues)
            {
                stringBuilder.Append($"{_metricName} {item.BuildMetrice()}\n");
            }

            // 各个环节中，不能出现 \r\n
            return stringBuilder.ToString();
        }


    }
}
