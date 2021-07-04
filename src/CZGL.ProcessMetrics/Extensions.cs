using CZGL.ProcessMetrics.Prometheus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace CZGL.ProcessMetrics
{
    public static class Extensions
    {

        public static void ProcessMetrices(this IEndpointRouteBuilder endpoints, string routePath = "/metrices")
        {
            endpoints.Map(routePath, async (HttpContext context) =>
            {
                var metrice = ProcessMetricsCore.Instance;
                context.Response.ContentType = "text/plain; version=0.0.4; charset=utf-8";
                await context.Response.WriteAsync(metrice.GetPrometheus());
            });

        }


    }
}
