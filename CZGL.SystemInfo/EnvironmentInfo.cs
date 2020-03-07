using CZGL.SystemInfo.Info;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using static CZGL.SystemInfo.TollHelper;

namespace CZGL.SystemInfo
{
    /// <summary>
    /// 获取程序运行信息的类
    /// </summary>
    public static class EnvironmentInfo
    {
        #region 获取信息

        /// <summary>
        /// 机器的资源信息
        /// </summary>
        /// <returns></returns>
        public static (string, KeyValuePair<string, object>[]) GetMachineInfo()
        {
            MachineRunInfo info = new MachineRunInfo();
            return GetValues(info);
        }

        /// <summary>
        /// 获取机器的资源信息
        /// </summary>
        /// <returns></returns>
        public static KeyValuePair<string, object>[] GetMachineInfoValue()
        {
            MachineRunInfo info = new MachineRunInfo();
            return GetValue(info);
        }


        /// <summary>
        /// 获取系统平台信息
        /// </summary>
        /// <returns></returns>
        public static (string, KeyValuePair<string, object>[]) GetSystemPlatformInfo()
        {
            SystemPlatformInfo info = new SystemPlatformInfo();
            return GetValues(info);
        }

        /// <summary>
        /// 获取系统平台信息
        /// </summary>
        /// <returns></returns>
        public static KeyValuePair<string, object>[] GetSystemPlatformInfoValue()
        {
            SystemPlatformInfo info = new SystemPlatformInfo();
            return GetValue(info);
        }




        /// <summary>
        /// 获取系统运行属性信息
        /// </summary>
        /// <returns></returns>
        public static (string, KeyValuePair<string, object>[]) GetSystemRunInfo()
        {
            SystemRunEvnInfo info = new SystemRunEvnInfo();
            return GetValues(info);
        }

        /// <summary>
        /// 获取系统运行属性信息
        /// </summary>
        /// <returns></returns>
        public static KeyValuePair<string, object>[] GetSystemRunInfoValue()
        {
            SystemRunEvnInfo info = new SystemRunEvnInfo();
            return GetValue(info);
        }


        /// <summary>
        /// 获取系统全部环境变量
        /// </summary>
        /// <returns></returns>
        public static KeyValuePair<string, object>[] GetEnvironmentVariables()
        {
            List<KeyValuePair<string, object>> list = new List<KeyValuePair<string, object>>();
            IDictionary environmentVariables = Environment.GetEnvironmentVariables();
            foreach (DictionaryEntry de in environmentVariables)
            {
                list.Add(new KeyValuePair<string, object>(de.Key.ToString(), de.Value));
            }
            return list.ToArray();
        }

        #endregion
    }
}
