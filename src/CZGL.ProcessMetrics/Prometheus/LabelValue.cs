using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZGL.ProcessMetrics.Prometheus
{
    /// <summary>
    /// 标签与值生成
    /// </summary>
    public class LabelValue : ILabel
    {
        private readonly Dictionary<string, string> Labels;
        protected decimal _Value = 0;
        protected string _Name = string.Empty;

        public LabelValue()
        {
            Labels = new Dictionary<string, string>();
        }

        public ILabel AddLabel(string labelName, string value)
        {
            Labels.Add(labelName, value);
            return this;
        }

        public ILabel Inc(decimal value)
        {
            _Value += value;
            return this;
        }

        public ILabel SetValue(decimal value, string name = null)
        {
            _Value = value;
            this._Name = name;
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

            stringBuilder.Append(_Value);
            if (!string.IsNullOrEmpty(_Name))
                stringBuilder.Append($" @{_Name}");

            return stringBuilder.ToString();
        }
    }
}
