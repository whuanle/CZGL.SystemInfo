using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using CZGL.SystemInfo;
using CZGL.SystemInfo.Info;
using env = CZGL.SystemInfo.EnvironmentInfo;

namespace Consoletest
{

    // 支持 Linux 和 Windows
    class Program
    {
        static void Main(string[] args)
        {
            // 注意，刚刚启动程序时，短时间内获取的信息不一定准确
            for (int i = 0; i < 5; i++)
            {
                Info();
                Console.WriteLine("-----------");
            }

            // 直接输出全部数据
            RefInfo();

            Console.ReadKey();
        }

        /// <summary>
        /// 手动获取需要的数据
        /// </summary>
        public static void Info()
        {
            // 注意，刚刚启动程序时，短时间内获取的信息不一定准确
            // 通过静态实例获取
            Console.WriteLine("当前进程已用内存" + MachineRunInfo.Instance.ThisUsedMem);

            // new实例获取
            MachineRunInfo m = new MachineRunInfo();
            Console.WriteLine("当前进程已用内存" + m.ThisUsedMem);
        }

        /// <summary>
        /// 获取所有信息的键值对
        /// </summary>
        public static void RefInfo()
        {
            // 注意，一些资源的单位都是 kb

            // 获取系统平台信息
            KeyValuePair<string, object>[] a = env.GetSystemPlatformInfoValue();
            // 获取系统运行属性信息
            KeyValuePair<string, object>[] b = env.GetSystemRunInfoValue();
            // 获取机器资源信息
            KeyValuePair<string, object>[] c = env.GetMachineInfoValue();
            // 获取系统所有环境变量
            KeyValuePair<string, object>[] d = env.GetEnvironmentVariables();

            Console.WriteLine("\n系统平台信息：\n");
            foreach (var item in a)
            {
                Console.WriteLine($"{item.Key}    :    {item.Value}");
            }

            Console.WriteLine("\n\n系统运行属性信息：\n");
            foreach (var item in b)
            {
                Console.WriteLine($"{item.Key}    :    {item.Value}");
            }

            Console.WriteLine("\n\n机器资源信息：\n");
            foreach (var item in c)
            {
                Console.WriteLine($"{item.Key}    :    {item.Value}");
            }

            Console.WriteLine("\n\n系统所有环境变量：\n");
            foreach (var item in d)
            {
                Console.WriteLine($"{item.Key}    :    {item.Value}");
            }
        }
    }
}