
using CZGL.SystemInfo;
using System.Net;

public class Program
{
    public static void Main(string[] args)
    {
        PrintNetworkInfo();
    }

    /// <summary>
    /// 打印所有网络信息
    /// </summary>
    public static void PrintNetworkInfo()
    {
        var ns = NetworkInfo.GetNetworkInfos();
        var type = typeof(NetworkInfo);
        foreach (var item in ns)
        {
            foreach (var p in type.GetProperties())
            {
                if (p.PropertyType == typeof(IPAddress[]))
                {
                    var v = p.GetValue(item) as IPAddress[];
                    Console.WriteLine($"{p.Name}:{string.Join(" ", v.Select(x => x.ToString()))}");
                }
                else
                {
                    Console.WriteLine($"{p.Name}:   {p.GetValue(item)}");
                }
            }
            Console.WriteLine("-----------------------");
        }
    }

}