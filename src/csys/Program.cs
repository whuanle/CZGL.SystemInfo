using CZGL.SystemInfo;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CZGLSystemInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("请输入命令");
            string command = "";
            while (true)
            {
                Console.WriteLine("+-------命令参考------------------------------+");
                Console.WriteLine("| 1. 输入 net 监控网络流量                     |");
                Console.WriteLine("| 2. 输入 test ，检查当前操作系统不兼容哪些 API    |");
                Console.WriteLine("+---------------------------------------------+");
                Console.Write("$> ");
                command = Console.ReadLine();
                if (command == Command.NET)
                    NET();

                else if (command == Command.TEST)
                    Test();
            }
        }


        /// <summary>
        /// 监控网络流量
        /// </summary>
        private static void NET()
        {
            var realIp = NetworkInfo.TryGetRealIpv4();
            if (realIp == null)
            {
                Console.WriteLine("未能获取网卡，操作终止");
                return;
            }

            Console.WriteLine($"IP 地址：{realIp}");

            var infos = NetworkInfo.GetNetworkInfos().ToArray();
            var info = infos.FirstOrDefault(x => x.UnicastAddresses.Any(x => x.MapToIPv4().ToString() == realIp.MapToIPv4().ToString()));
            if (info == null)
            {
                Console.WriteLine("未能获取网卡，操作终止");
                return;
            }
            Console.WriteLine("\r\n+++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine($"    网卡名称     {info.Name}");
            try
            {
                Console.WriteLine($"    网络链接速度 {info.Speed / 1000 / 1000} Mbps");
            }
            catch { }

            Console.WriteLine("开始监控流量，如需停止，可以按下任意键");
            CancellationTokenSource source = new CancellationTokenSource();
            var token = source.Token;
            Thread thread = new Thread(() =>
            {
                try
                {
                    var oldRate = info.GetIpv4Speed();
                    while (true)
                    {
                        if (token.IsCancellationRequested)
                            return;

                        Thread.Sleep(1000);
                        var newRate = info.GetIpv4Speed();
                        var speed = NetworkInfo.GetSpeed(oldRate, newRate);
                        Console.WriteLine($"网络上传速度：{speed.Sent.Size} {speed.Sent.SizeType}/S");
                        Console.WriteLine($"网络下载速度：{speed.Received.Size} {speed.Received.SizeType}/S");
                        oldRate = newRate;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    Console.WriteLine("你的操作系统不支持此功能！按下任意键继续输入命令");
                    return;
                }
            });
            thread.Start();
            Console.ReadKey();
            source.Cancel();
        }

        private static void Test()
        {
            var type1 = typeof(SystemPlatformInfo);
            foreach (var item in type1.GetProperties())
            {
                Console.WriteLine("----------------------------");
                try
                {
                    _ = item.GetValue(null);
                    Console.WriteLine($"{type1.Name} {item.Name} √");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{type1.Name} {item.Name} ×");
                    Console.WriteLine(ex.ToString());
                }
            }

            var type2 = typeof(Process);
            var thisProcess = Process.GetCurrentProcess();
            thisProcess.Refresh();
            foreach (var item in type2.GetProperties())
            {
                Console.WriteLine("----------------------------");
                try
                {
                    _ = item.GetValue(thisProcess);
                    Console.WriteLine($"{type2.Name} {item.Name} √");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{type2.Name} {item.Name} ×");
                    Console.WriteLine(ex.ToString());
                }
            }


            var type3 = typeof(NetworkInfo);
            var realIp = NetworkInfo.TryGetRealIpv4();
            if (realIp == null)
            {
                Console.WriteLine("未能获取网卡，操作终止");
                return;
            }

            Console.WriteLine($"IP 地址：{realIp}");

            var infos = NetworkInfo.GetNetworkInfos().ToArray();
            var network = infos.FirstOrDefault(x => x.UnicastAddresses.Any(x => x.MapToIPv4().ToString() == realIp.MapToIPv4().ToString()));
            if (network == null)
            {
                Console.WriteLine("未能获取网卡，操作终止");
                return;
            }
            foreach (var item in type3.GetProperties())
            {
                Console.WriteLine("----------------------------");
                try
                {
                    _ = item.GetValue(network);
                    Console.WriteLine($"{type3.Name} {item.Name} √");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{type3.Name} {item.Name} ×");
                    Console.WriteLine(ex.ToString());
                }
            }

            var type4 = typeof(DiskInfo);
            var disk = DiskInfo.GetDisks()[1];
            foreach (var item in type4.GetProperties())
            {
                Console.WriteLine("----------------------------");
                try
                {
                    _ = item.GetValue(disk);
                    Console.WriteLine($"{type4.Name} {item.Name} √");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{type4.Name} {item.Name} ×");
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }


    public static class Command
    {
        public const string Info = "info";
        public const string NET = "net";
        public const string NETINFO = "netinfo";
        public const string TEST = "test";
    }
}
