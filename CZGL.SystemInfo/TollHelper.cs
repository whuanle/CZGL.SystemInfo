using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace CZGL.SystemInfo
{
    public static class TollHelper
    {
      #region 工具

        /// <summary>
        /// 获取某个类型的值以及名称
        /// </summary>
        /// <typeparam name="TInfo"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public static (string, KeyValuePair<string, object>[]) GetValues<TInfo>(TInfo info)
        {
            List<KeyValuePair<string, object>> list = new List<KeyValuePair<string, object>>();
            Type type = info.GetType();
            PropertyInfo[] pros = type.GetProperties();
            foreach (var item in pros)
            {
                var name = GetInfoNameValue(item.GetCustomAttributesData());
                var value = GetPropertyInfoValue(item, info);
                list.Add(new KeyValuePair<string, object>(name, value));
            }
            return
                (GetInfoNameValue(info.GetType().GetCustomAttributesData()),
                list.ToArray());
        }

        /// <summary>
        /// 只获取值
        /// </summary>
        /// <typeparam name="TInfo"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public static KeyValuePair<string, object>[] GetValue<TInfo>(TInfo info)
        {
            if (info == null) return default;
            List<KeyValuePair<string, object>> list = new List<KeyValuePair<string, object>>();
            Type type = info.GetType();
            PropertyInfo[] pros = type.GetProperties();
            foreach (var item in pros)
            {
                var attrs = item.GetCustomAttributesData();
                if(attrs==null||attrs.Count==0) continue;
                var name =  (System.Threading.Thread.CurrentThread.CurrentCulture.Name=="zh-CN")?
                    GetInfoNameValue(attrs):item.Name;
                if(string.IsNullOrEmpty(name)) continue;
                var value = GetPropertyInfoValue(item, info);
                list.Add(new KeyValuePair<string, object>(name, value));
            }
            return list.ToArray();
        }
        /// <summary>
        /// 获取 [InfoName] 特性的属性 ChinaName 的值 或设置为属性名称
        /// </summary>
        /// <param name="attrs"></param>
        /// <returns></returns>
        public static string GetInfoNameValue(IList<CustomAttributeData> attrs)
        {
            var argument = attrs.FirstOrDefault(x => x.AttributeType.Name == nameof(InfoNameAttribute)).NamedArguments;
                if (argument == null)
                    return null;
                return argument.FirstOrDefault(x => x.MemberName == nameof(InfoNameAttribute.ChinaName)).TypedValue.Value.ToString();
        }

        /// <summary>
        /// 获取属性的值
        /// </summary>
        /// <param name="info"></param>
        /// <param name="obj">实例</param>
        /// <returns></returns>
        public static object GetPropertyInfoValue(PropertyInfo info, object obj)
        {
            return info.GetValue(obj);
        }
        
        #endregion
    }
}