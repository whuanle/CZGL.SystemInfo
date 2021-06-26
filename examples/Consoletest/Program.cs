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
            var tmp = NetworkInterface.GetAllNetworkInterfaces();
            Console.ReadKey();
        }
    }
}