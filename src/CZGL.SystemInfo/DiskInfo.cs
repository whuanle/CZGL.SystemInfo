using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CZGL.SystemInfo
{
    /// <summary>
    /// 磁盘信息
    /// </summary>
    public class DiskInfo
    {
        private readonly DriveInfo _info;
        public DriveInfo DriveInfo => _info;

        private DiskInfo(DriveInfo info)
        {
            _info = info;
        }

        /// <summary>
        /// Drive Id<br />
        /// 驱动器名称
        /// <para>ex: C:\</para>
        /// </summary>
        public string Id => _info.Name;

        /// <summary>
        /// Drive Name,Volume label
        /// 磁盘名称
        /// <para>ex:<br />
        /// Windows:system<br />
        /// Linux:  /dev
        /// </para>
        /// </summary>
        public string Name => _info.Name;

        /// <summary>
        /// Gets the drive type<br />
        /// 获取驱动器类型
        /// </summary>
        /// <remarks>获取驱动器类型，如 CD-ROM、可移动、网络或固定</remarks>
        public DriveType DriveType => _info.DriveType;

        /// <summary>
        ///  File system<br />
        ///  文件系统
        ///  <para>
        ///  Windows:NTFS、 CDFS...
        ///  Linux:  rootfs、tmpfs、binfmt_misc...
        ///  </para>
        /// </summary>
        public string FileSystem => _info.DriveFormat;

        /// <summary>
        /// Indicates the amount of available free space on a drive, in bytes<br />
        /// 可用空闲空间总量（以字节为单位）
        /// </summary>
        public long FreeSpace => _info.AvailableFreeSpace;

        /// <summary>
        /// Gets the total size of storage space on a drive, in bytes<br />
        /// 总空间大小
        /// </summary>
        public long TotalSize => _info.TotalSize;

        public long UsedSize => TotalSize - FreeSpace;

        /// <summary>
        /// 获取本地磁盘信息
        /// </summary>
        /// <returns></returns>
        public static DiskInfo[] GetDisks()
        {
            return DriveInfo.GetDrives().Select(x => new DiskInfo(x)).ToArray();
        }

        /// <summary>
        /// 获取 Docker 运行的容器，其容器文件系统在主机中的存储位置
        /// </summary>
        /// <returns></returns>
        public static DiskInfo[] GetContainerMerge()
        {
            return DriveInfo.GetDrives()
                 .Where(x => x.DriveFormat.Equals("overlay", StringComparison.OrdinalIgnoreCase) && x.DriveFormat.Contains("docker"))
                 .Select(x => new DiskInfo(x)).ToArray();
        }


        /// <summary>
        /// 筛选出能够使用的真正的磁盘
        /// </summary>
        /// <returns></returns>
        public static DiskInfo[] GetRealDisk()
        {
            var disks = DriveInfo.GetDrives()
            .Where(x =>
            x.DriveType == System.IO.DriveType.Fixed &&
            x.TotalSize != 0 && x.DriveFormat != "overlay");

            return disks.Select(x => new DiskInfo(x))
                .Distinct(new DiskInfoEquality()).ToArray();
        }

        /// <summary>
        /// 筛选重复项
        /// </summary>
        private class DiskInfoEquality : IEqualityComparer<DiskInfo>
        {
            public bool Equals(DiskInfo x, DiskInfo y)
            {
                return x.Id == y.Id;
            }

            public int GetHashCode(DiskInfo obj)
            {
                return obj.Id.GetHashCode();
            }
        }
    }

}
