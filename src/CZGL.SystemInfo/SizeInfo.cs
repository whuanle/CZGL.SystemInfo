using System;

namespace CZGL.SystemInfo
{
    /// <summary>
    /// 大小信息
    /// </summary>
    public struct SizeInfo
    {
        /// <summary>
        /// Byte 长度
        /// </summary>
        public long ByteLength { get; private set; }

        /// <summary>
        /// 大小
        /// </summary>
        public decimal Size { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public UnitType SizeType { get; set; }


        /// <summary>
        /// 将字节单位转换为合适的单位
        /// </summary>
        /// <param name="byteLength">字节长度</param>
        /// <returns></returns>
        public static SizeInfo Get(long byteLength)
        {
            UnitType unit = 0;
            decimal number = byteLength;
            if (byteLength < 1000)
            {
                return new SizeInfo()
                {
                    ByteLength = byteLength,
                    Size = byteLength,
                    SizeType = UnitType.B
                };
            }
            // 避免出现 1023B 这种情况；这样 1023B 会显示 0.99KB
            while (Math.Round(number / 1000) >= 1)
            {
                number = number / 1024;
                unit++;
            }

            return new SizeInfo
            {
                Size = Math.Round(number, 2),
                SizeType = unit,
                ByteLength = byteLength
            };

            throw new Exception();
        }
    }
}
