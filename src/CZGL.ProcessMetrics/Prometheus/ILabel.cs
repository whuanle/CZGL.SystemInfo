using System;
using System.Collections.Generic;
using System.Text;

namespace CZGL.ProcessMetrics.Prometheus
{
    public interface ILabel
    {
        ILabel AddLabel(string labelName, string value);
        ILabel SetValue(decimal value, string name = null);
    }
}
