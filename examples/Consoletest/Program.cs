using System;
using System.IO;

namespace Consoletest
{
    public class Program
    {
        static void Main()
        {
            var text = File.ReadAllText("/proc/stat");
            Console.WriteLine(text);
        }
    }
}