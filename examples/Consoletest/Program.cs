using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using CZGL.SystemInfo;

namespace Consoletest
{

    // 支持 Linux 和 Windows
    class Program
    {
        public static void Main()
        {
            var z = (NetworkInterface.GetAllNetworkInterfaces()
                                                          .Where(x =>
                                                          x.NetworkInterfaceType != NetworkInterfaceType.Loopback
                                              && x.NetworkInterfaceType != NetworkInterfaceType.Ethernet)).ToList();
            Console.WriteLine(z.Count);
            foreach (var item in z)
            {
                Console.WriteLine(item.OperationalStatus);
            }


            Console.ReadKey();
        }
    }
}