using System;
using System.Collections.Generic;
using System.Text;

namespace CZGL.SystemInfo
{
    /// <summary>
    /// 大小信息
    /// </summary>
    public struct SizeInfo
    {
        /// <summary>
        /// 大小
        /// </summary>
        public decimal Size { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public SizeType SizeType { get; set; }


        /// <summary>
        /// 字节单位转换，以 1024 为一个级别
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static SizeInfo Get(long size)
        {
            SizeInfo info = new SizeInfo();

            if (size < 1024)
            {
                return new SizeInfo
                {
                    Size = (decimal)(size),
                    SizeType = SizeType.B
                };
            }
            if (size < 1024 * 1024)
            {
                return new SizeInfo
                {
                    Size = Math.Round((decimal)size / 1024, 1),
                    SizeType = SizeType.KB
                };
            }
            if (size < 1024 * 1024 * 1024)
            {
                return new SizeInfo
                {
                    Size = Math.Round((decimal)(size >> 19) / 2),
                    SizeType = SizeType.MB
                };
            }

            if (size < (long)1024 * 1024 * 1024 * 1024)
            {
                return new SizeInfo
                {
                    Size = Math.Round((decimal)(size >> 29) / 2),
                    SizeType = SizeType.GB
                };
            }

            if (size < (long)1024 * 1024 * 1024 * 1024 * 1024)
            {
                return new SizeInfo
                {
                    Size = Math.Round((decimal)(size >> 39) / 2),
                    SizeType = SizeType.TB
                };
            }

            if (size < (long)1024 * 1024 * 1024 * 1024 * 1024 * 1024)
            {
                return new SizeInfo
                {
                    Size = Math.Round((decimal)(size >> 49) / 2),
                    SizeType = SizeType.TB
                };
            }

            throw new Exception();
        }
    }
}
