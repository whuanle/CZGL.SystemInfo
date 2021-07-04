using CZGL.SystemInfo;
using System;
using System.Diagnostics;
using System.Linq;
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
                Console.WriteLine("| 1. 输入 netinfo 查看网络详情                  |");
                Console.WriteLine("| 2. 输入 nett 监控网络流量                     |");
                Console.WriteLine("| 3. 输入 test ，检查当前操作系统不兼容哪些 API    |");
                Console.WriteLine("| 4. 输入 ps 查看进程信息                       |");
                Console.WriteLine("+---------------------------------------------+");
                Console.Write("$> ");
                command = Console.ReadLine();
                // netinfo
                if (command == Command.NETINFO)
                    NETINFO();

                else if (command == Command.NET_T)
                    NET_T();

                else if (command == Command.TEST)
                    Test();

                else if (command == Command.PS)
                    PS();
            }
        }



        /// <summary>
        /// 当前联网的网卡信息
        /// </summary>
        private static void NETINFO()
        {
            var info = NetworkInfo.GetRealNetworkInfos().First();
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

            Console.WriteLine($"    Ipv6       {info.AddressIpv6.ToString()}");
            Console.WriteLine($"    Ipv4       {info.AddressIpv4.ToString()}");
            Console.WriteLine($"    DNS        {string.Join(',', info.DNSAddresses.Select(x => x.ToString()).ToArray())}");
            try
            {
                Console.WriteLine($"    上行流量统计 {info.SendLength / 1024 / 1024} MB");
                Console.WriteLine($"    下行流量统计 {info.ReceivedLength / 1024 / 1024} MB");
            }
            catch { }
            Console.WriteLine($"    网络类型     {info.NetworkType}");
            Console.WriteLine($"    网卡MAC     {info.Mac}");
            Console.WriteLine($"    网卡信息     {info.Trademark}");
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++");
        }

        /// <summary>
        /// 监控网络流量
        /// </summary>
        private static void NET_T()
        {
            Console.WriteLine("开始监控流量，如需停止，可以按下任意键");
            var info = NetworkInfo.GetRealNetworkInfos().First();
            bool isStop = false;
            Thread thread = new Thread(() =>
            {
                try
                {
                    var speed = new InternetSpeed();
                    while (true)
                    {
                        if (isStop)
                            return;
                        var tmp = info.GetInternetSpeed(ref speed,1000);
                        if (isStop)
                            return;

                        var send = tmp.Sent;
                        var received = tmp.Received;

                        Console.WriteLine($"网络上传速度：{send.Size} {send.SizeType}/S");
                        Console.WriteLine($"网络下载速度：{received.Size} {received.SizeType}/S");

                        Thread.Sleep(500);
                    }
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex);
                    Console.WriteLine("你的操作系统不支持此功能！按下任意键继续输入命令");
                    return;
                }
            });
            thread.Start();
            Console.ReadKey();
            isStop = true;

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

            var type2 = typeof(ProcessInfo);
            var thisProcess = ProcessInfo.GetCurrentProcess();
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
            var network = NetworkInfo.GetRealNetworkInfos();
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

        private static void PS()
        {
            while (true)
            {
                Console.WriteLine("输入需要查看的进程 id ，输入all列出所有进程，输入0结束");
                Console.Write("$> ");
                string tmp = Console.ReadLine();
                if (!int.TryParse(tmp, out var pid))
                {
                    if (tmp == "all")
                    {
                        Console.WriteLine("-----------------");
                        Console.WriteLine("-进程id------进程名称---");
                        foreach (var item in ProcessInfo.GetProcessList())
                        {
                            Console.WriteLine($"{item.Key}   {item.Value}");
                        }
                    }
                    continue;
                }

                if (pid == 0)
                    return;

                Console.WriteLine("-----------------");
                Console.WriteLine("正在监控此进程的资源，如需取消监控，按下任意键即可");

                bool isStop = false;
                var info = ProcessInfo.GetProcess(pid);
                info.Refresh();
                Thread thread = new Thread(() =>
                {
                    while (true)
                    {
                        if (isStop)
                            return;
                        decimal count = ProcessInfo.GetCpuUsage(info);

                        Console.WriteLine($"进程名称 :     {info.ProcessName}");
                        Console.WriteLine($"所属模块 :     {info.MainModule} Byte");
                        Console.WriteLine($"进程启动时间 :  {info.StartTime.ToString("yyyy-MM-dd HH:mm:ss")}");
                        Console.WriteLine($"CPU :         {count}% ");
                        Console.WriteLine($"已用内存 :      {info.PhysicalUsedMemory} Byte");

                        if (isStop)
                            return;
                        info.Refresh();
                    }
                });
                thread.Start();
                Console.ReadKey();
                isStop = true;
            }
        }
    }


    public static class Command
    {
        public const string Info = "info";
        public const string NET = "net";
        public const string NET_T = "nett";
        public const string NETINFO = "netinfo";
        public const string TEST = "test";
        public const string PS = "ps";
    }
}
