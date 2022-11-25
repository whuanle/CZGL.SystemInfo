
using CZGL.SystemInfo;
using CZGL.SystemInfo.CPU;
using CZGL.SystemInfo.CPU.Windows;
using System.Net;
using System.Numerics;

public class Program
{
    public static void Main(string[] args)
    {
        CPUTime v1 = CPUHelper.GetCPUTime();

        while (true)
        {
            Thread.Sleep(1000);
            var v2 = CPUHelper.GetCPUTime();
            var value = CPUHelper.CalculateCPULoad(v1, v2);
            v1 = v2;
            Console.Clear();
            Console.WriteLine($"{(int)(value * 100)} %");
        }
    }


    ///// <summary>
    ///// 打印所有网络信息
    ///// </summary>
    //public static void PrintNetworkInfo()
    //{
    //    var ns = NetworkInfo.GetNetworkInfos();
    //    var type = typeof(NetworkInfo);
    //    foreach (var item in ns)
    //    {
    //        foreach (var p in type.GetProperties())
    //        {
    //            if (p.PropertyType == typeof(IPAddress[]))
    //            {
    //                var v = p.GetValue(item) as IPAddress[];
    //                Console.WriteLine($"{p.Name}:{string.Join(" ", v.Select(x => x.ToString()))}");
    //            }
    //            else
    //            {
    //                Console.WriteLine($"{p.Name}:   {p.GetValue(item)}");
    //            }
    //        }
    //        Console.WriteLine("-----------------------");
    //    }
    //}

}