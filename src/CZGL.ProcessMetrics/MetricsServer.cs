using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace CZGL.ProcessMetrics
{
    /// <summary>
    /// 启动 Metrics 服务
    /// </summary>
    public class MetricsServer
    {
        private readonly HttpListener listener;
        private bool _stop = false;
        private readonly ProcessMetricsCore _metricsCore;


        /// <summary>
        /// Metrics 服务
        /// </summary>
        /// <param name="option">设置</param>
        /// <param name="url">地址</param>
        public MetricsServer(Action<MetricsOption> option = null, string url = "http://*:1234/metrics/")
        {
            MetricsOption metricsOption = new MetricsOption();
            if (option != null)
            {
                option.Invoke(metricsOption);
            }

            _metricsCore = new ProcessMetricsCore(metricsOption);

            listener = new HttpListener();
            Trace.WriteLine($"Metrics now listening on: {url}");
            listener.Prefixes.Add(url);
        }

        /// <summary>
        /// Metrics 服务
        /// </summary>
        /// <param name="port">监控地址</param>
        public MetricsServer(string url = "http://*:1234/metrics/") : this(null, url) { }

        /// <summary>
        /// 启用此方法后，会阻塞运行
        /// </summary>
        public void Start()
        {
            listener.Start();
            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                if (_stop) listener.Stop();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                if (_stop) listener.Stop();

                byte[] buffer = Encoding.UTF8.GetBytes(_metricsCore.GetPrometheus().Result);
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
        }

        public void Stop()
        {
            _stop = true;
        }
    }
}
