using System;
using System.Collections.Generic;
using CZGL.SystemInfo.Linux;

namespace ConsoleLinux
{
    // 只支持 Linux
    class Program
    {
        static void Main(string[] args)
        {
            DynamicInfo info = new DynamicInfo();

            //Get(info);
            RefGet(info);
            Console.ReadKey();
        }

        public static void Get(DynamicInfo info)
        {
            var item = info.GetTasks();
            Console.WriteLine("系统中共有进程数    :" + item.Total);
        }

        public static void RefGet(DynamicInfo info)
        {
            // 获取进程统计
            KeyValuePair<string, object>[] a = info.GetRefTasks();

            // 获取CPU资源统计
            KeyValuePair<string, object>[] b = info.GetRefCpuState();

            // 获取内存统计
            KeyValuePair<string, object>[] c = info.GetRefMem();

            // 获取虚拟内存统计
            KeyValuePair<string, object>[] d = info.GetRefSwap();

            Dictionary<int, PidInfo> dic = info.GetPidInfo();

            Console.WriteLine("\n进程统计：\n");
            foreach (var item in a)
            {
                Console.WriteLine($"{item.Key}    :    {item.Value}");
            }

            Console.WriteLine("\n\nCPU资源统计：\n");
            foreach (var item in b)
            {
                Console.WriteLine($"{item.Key}    :    {item.Value}");
            }

            Console.WriteLine("\n\n内存统计：\n");
            foreach (var item in c)
            {
                Console.WriteLine($"{item.Key}    :    {item.Value}");
            }

            Console.WriteLine("\n\n获取虚拟内存统计：\n");
            foreach (var item in d)
            {
                Console.WriteLine($"{item.Key}    :    {item.Value}");
            }

            Console.WriteLine("\n\n 各个进程使用的资源：\n");
            Console.WriteLine("  进程Id  进程名称  所属用户    优化级  高低优先级  虚拟内存   物理内存   共享内存 进程状态  占用系统CPU(%)   占用内存(%d) ");

            foreach (var item in dic)
            {
                Console.WriteLine($"{item.Key}  {item.Value.Command}  {item.Value.User}  {item.Value.PR}  " +
                                  $"{item.Value.Nice}  {item.Value.VIRT}  {item.Value.RES}  {item.Value.SHR}  " +
                                  $"{item.Value.State}  {item.Value.CPU}  {item.Value.Mem}");
            }
        }
    }
}