using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CZGL.ProcessMetrics
{

    public class MetricsServer
    {
        private readonly HttpListener listener;
        private bool _stop = false;

        /// <summary>
        /// Metrics 服务
        /// </summary>
        /// <param name="port">监控地址</param>
        public MetricsServer(string url = "http://*:1234/metrics/")
        {
            listener = new HttpListener();
            listener.Prefixes.Add(url);
        }

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

                var processMetricsCore = ProcessMetricsCore.Instance;
                byte[] buffer = Encoding.UTF8.GetBytes(processMetricsCore.GetPrometheus().Result);
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
