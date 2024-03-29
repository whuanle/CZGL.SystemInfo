﻿using CZGL.ProcessMetrics;
namespace Consoletest
{
    class Program
    {
        // 需要使用管理员权限
        public static void Main()
        {
            Console.WriteLine("感谢你的帮助，Metrics 正在启动中。。。");
            MetricsPush metricsPush = new MetricsPush(url: "http://106.12.123.126:9091", option: options =>
            {
                // 监控 CLR 中的事件
                options.ListenerNames.Add(EventNames.System_Runtime);
                options.ListenerNames.Add(EventNames.AspNetCore_Http_Connections);
            });

            while (true)
            {
                var str = metricsPush.PushAsync().Result;
                Console.WriteLine(str);
                Thread.Sleep(1000);
            }

            Console.ReadKey();
        }
    }
}