
using CZGL.SystemInfo;

public class Program
{
    public static void Main(string[] args)
    {
        CPUTime v1 = CPUHelper.GetCPUTime();
        var network = NetworkInfo.TryGetRealNetworkInfo();
        var oldRate = network.GetIpv4Speed();
        while (true)
        {
            Thread.Sleep(1000);
            var v2 = CPUHelper.GetCPUTime();
            var value = CPUHelper.CalculateCPULoad(v1, v2);
            v1 = v2;

            var memory = MemoryHelper.GetMemoryValue();
            var newRate = network.GetIpv4Speed();
            var speed = NetworkInfo.GetSpeed(oldRate, newRate);
            oldRate = newRate;
            Console.Clear();
            Console.WriteLine($"CPU:    {(int)(value * 100)} %");
            Console.WriteLine($"已用内存：{memory.UsedPercentage}%");
            Console.WriteLine($"上传：{speed.Sent.Size} {speed.Sent.SizeType}/S    下载：{speed.Received.Size} {speed.Received.SizeType}/S");
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