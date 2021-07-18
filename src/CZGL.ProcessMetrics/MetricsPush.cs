using CZGL.SystemInfo;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CZGL.ProcessMetrics
{
    /// <summary>
    /// Metrics 推送服务
    /// </summary>
    public class MetricsPush
    {
        private readonly ProcessMetricsCore _metricsCore;
        private readonly string _url;

        /// <summary>
        /// 推送
        /// </summary>
        /// <param name="option">设置</param>
        /// <param name="url">推送地址</param>
        public MetricsPush(string url = "http://127.0.0.1:9091", Action<MetricsOption> option = null)
        {
            MetricsOption metricsOption = new MetricsOption();
            if (option != null)
            {
                option.Invoke(metricsOption);
            }

            _metricsCore = new ProcessMetricsCore(metricsOption);
            _url = $"{url}/metrics/job/{SystemPlatformInfo.MachineName.ToLower() + metricsOption.JobName}";
        }

        public async Task<int> PushAsync()
        {
            var content = await _metricsCore.GetPrometheus();
            return await PushMetrics(content);
        }

        private async Task<int> PushMetrics(string text)
        {
            var httpclientHandler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, error) => true,
            };

            var jsonContent = new StringContent(text);
            jsonContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");

            using (var httpClient = new HttpClient(httpclientHandler))
            {
                var result = await httpClient.PutAsync(_url, jsonContent);
                return (int)result.StatusCode;
            }
        }
    }
}
