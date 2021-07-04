using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZGL.ProcessMetrics.Prometheus
{
    public class LabelValue : ILabel
    {
        private readonly Dictionary<string, string> Labels;
        protected readonly List<KeyValuePair<decimal, string>> Values;

        public LabelValue()
        {
            Labels = new Dictionary<string, string>();
            Values = new List<KeyValuePair<decimal, string>>();
        }

        public ILabel AddLabel(string labelName, string value)
        {
            Labels.Add(labelName, value);
            return this;
        }

        public ILabel AddValue(decimal value, string name = null)
        {
            Values.Add(new KeyValuePair<decimal, string>(value, name));
            return this;
        }

        public string BuildMetrice()
        {
            StringBuilder stringBuilder = new StringBuilder();
            var labelsValues = Labels.ToArray();

            if (labelsValues.Length != 0)
            {
                stringBuilder.Append("{");
                for (int i = 0; i < labelsValues.Length; i++)
                {
                    stringBuilder.Append($"{labelsValues[i].Key}=\"{labelsValues[i].Value}\"");
                    if (i < labelsValues.Length - 1)
                        stringBuilder.Append(",");
                }
                stringBuilder.Append("} ");
            }

            var values = Values.ToArray();
            if (values.Length != 0)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    stringBuilder.Append(values[i].Key);
                    if (values[i].Value != null)
                        stringBuilder.Append($" @{values[i].Value}");

                    if (i < values.Length - 1)
                        stringBuilder.Append(" ");
                }
            }
            return stringBuilder.ToString();
        }
    }
}
