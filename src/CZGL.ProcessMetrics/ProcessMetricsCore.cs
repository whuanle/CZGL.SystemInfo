using CZGL.ProcessMetrics.Counters;
using CZGL.ProcessMetrics.Prometheus;
using CZGL.SystemInfo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;

namespace CZGL.ProcessMetrics
{
    /// <summary>
    /// 监控收集
    /// </summary>
    public class ProcessMetricsCore
    {
        public List<IMerticsSource> _merticsSources;
        private readonly List<PrometheusFormat> prometheusFormats;
        private readonly MetricsOption _option;

        public ProcessMetricsCore() : this(null) { }

        public ProcessMetricsCore(MetricsOption option)
        {
            if (option == null)
                option = new MetricsOption();
            _option = option;

            prometheusFormats = new List<PrometheusFormat>();
            _merticsSources = new List<IMerticsSource>();

            var assemblies = option.Assemblies;
            option.Assemblies.Add(Assembly.GetExecutingAssembly());

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var item in types)
                {
                    var source = item.GetInterface(nameof(IMerticsSource));
                    if (source == null || item.IsInterface)
                        continue;

                    try
                    {
                        var obj = (IMerticsSource)Activator.CreateInstance(item);
                        _merticsSources.Add(obj);
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError(ex.ToString(), item);
                    }
                }
            }
        }

        /// <summary>
        /// 创建一个累加计数器
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="describetion">描述</param>
        /// <param name="value">初始值</param>
        /// <returns></returns>
        public Counter CreateCounter(string name, string describetion, decimal value)
        {
            var gauge = new Counter(name, describetion, value, _option);
            prometheusFormats.Add(gauge);
            return gauge;
        }

        /// <summary>
        /// 普通记录器
        /// </summary>
        /// <param name="name"><名称/param>
        /// <param name="describetion">描述</param>
        /// <returns></returns>
        public Gauge CreateGauge(string name, string describetion)
        {
            var gauge = new Gauge(name, describetion, _option);
            prometheusFormats.Add(gauge);
            return gauge;
        }

        /// <summary>
        /// 获取监控信息
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetPrometheus()
        {

            List<Task> metricsTasks = new List<Task>();

            foreach (var item in _merticsSources)
            {
                var task = item.InvokeAsync(this);
                metricsTasks.Add(task);
            }

            await Task.WhenAll(metricsTasks);

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in prometheusFormats)
            {
                try
                {
                    stringBuilder.Append(item.BuildMetrice());
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.ToString(), item);
                }
            }
            stringBuilder.Append("\n");

            prometheusFormats.Clear();
            return stringBuilder.ToString();
        }
    }
}
