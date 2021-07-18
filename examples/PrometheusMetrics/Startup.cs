using CZGL.ProcessMetrics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;

namespace PrometheusMetrics
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            new Thread(() =>
            {
                List<Stream> streams = new List<Stream>();
                int a = 0;
                while (true)
                {
                    try
                    {
                        HttpClient httpClient = new HttpClient();
                        var message = httpClient.GetAsync("https://www.baidu.com/?tn=62095104_26_oem_dg").Result;
                        var content = message.Content;
                        streams.Add(content.ReadAsStreamAsync().Result);
                        a++;
                        Thread.Sleep(800);
                        if (a % 10 == 0)
                            streams.Clear();
                    }
                    catch { }
                }
            }).Start();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.ProcessMetrices("/metrics", options =>
                {
                    // 监控 CLR 中的事件
                    options.ListenerNames.Add(EventNames.System_Runtime);
                    options.ListenerNames.Add(EventNames.AspNetCore_Http_Connections);

                    // options.Labels.Add("other", "自定义标识");

                    // 自定义要监控的数据源
                    options.Assemblies.Add(typeof(CZGL.ProcessMetrics.MetricsPush).Assembly);
                });

            });
        }
    }
}
