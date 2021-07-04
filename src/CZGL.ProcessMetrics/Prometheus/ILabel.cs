using System;
using System.Collections.Generic;
using System.Text;

namespace CZGL.ProcessMetrics.Prometheus
{
    public interface ILabel
    {
        public ILabel AddLabel(string labelName,string value);
        public ILabel AddValue(decimal value, string name = null);
    }
}
