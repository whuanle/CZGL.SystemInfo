using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;

namespace CZGL.ProcessMetrics
{
    public static class Extensions
    {
        private static ProcessMetricsCore _processMetricsCore;

        /// <summary>
        /// 设置 Metrics 访问服务
        /// </summary>
        /// <param name="endpoints"></param>
        /// <param name="routePath"></param>
        /// <param name="option"></param>
        public static void ProcessMetrices(this IEndpointRouteBuilder endpoints, string routePath = "/metrics", Action<MetricsOption> option = null)
        {
            MetricsOption metricsOption = new MetricsOption();
            if (option != null)
            {
                option.Invoke(metricsOption);
            }

            _processMetricsCore = new ProcessMetricsCore(metricsOption);
            endpoints.Map(routePath, async (HttpContext context) =>
            {
                context.Response.ContentType = "text/plain; version=0.0.4; charset=utf-8";
                await context.Response.WriteAsync(_processMetricsCore.GetPrometheus().Result);
            });
        }
    }
}
