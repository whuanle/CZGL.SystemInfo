using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace CZGL.SystemInfo
{

    /// <summary>
    /// Linux
    /// </summary>
    public static class LinuxCPU
    {
        const string Path = "/proc/stat";

        /// <summary>
        /// 获取 CPU 时间
        /// </summary>
        /// <returns></returns>
        public static CPUTime GetCPUTime()
        {
            ulong IdleTime = 0;
            ulong SystemTime = 0;
            try
            {
                var text = File.ReadAllLines(Path);

                foreach (var item in text)
                {
                    if (!item.StartsWith("cpu")) continue;
#if NET6_0_OR_GREATER
                    var values = item.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray();
                    SystemTime += (ulong)(values[1..].Select(x => decimal.Parse(x)).Sum());
#else
                    var values = item.Split(new char[] { ' '}, StringSplitOptions.RemoveEmptyEntries).ToArray();
                    SystemTime += (ulong)(values.ToList().GetRange(1, values.Length).Select(x => decimal.Parse(x)).Sum());
#endif

                    IdleTime += ulong.Parse(values[4]);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                Debug.Assert(false, ex.Message);
                throw new PlatformNotSupportedException($"{RuntimeInformation.OSArchitecture.ToString()}    {Environment.OSVersion.Platform.ToString()} {Environment.OSVersion.ToString()}");
            }

            return new CPUTime(IdleTime, SystemTime);
        }
    }


}