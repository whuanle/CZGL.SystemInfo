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


        /// <summary>
        /// 获取磁盘类
        /// </summary>
        public DriveInfo DriveInfo => _info;

        private DiskInfo(DriveInfo info)
        {
            _info = info;
        }

        /// <summary>
        /// 驱动器名称
        /// <para>ex: C:\</para>
        /// </summary>
        public string Id => _info.Name;

        /// <summary>
        /// 磁盘名称
        /// <para>ex:<br />
        /// Windows:    system<br />
        /// Linux:  /dev
        /// </para>
        /// </summary>
        public string Name => _info.Name;

        /// <summary>
        /// 获取驱动器类型
        /// </summary>
        /// <remarks>获取驱动器类型，如 CD-ROM、可移动、网络或固定</remarks>
        public DriveType DriveType => _info.DriveType;

        /// <summary>
        ///  文件系统
        ///  <para>
        ///  Windows:   NTFS、 CDFS...<br />
        ///  Linux:  rootfs、tmpfs、binfmt_misc...
        ///  </para>
        /// </summary>
        public string FileSystem => _info.DriveFormat;

        /// <summary>
        /// 磁盘剩余容量（以字节为单位）
        /// </summary>
        public long FreeSpace => _info.AvailableFreeSpace;

        /// <summary>
        /// 磁盘总容量（以字节为单位）
        /// </summary>
        public long TotalSize => _info.TotalSize;

        /// <summary>
        /// 磁盘剩余可用容量
        /// </summary>
        public long UsedSize => TotalSize - FreeSpace;

        /// <summary>
        /// 磁盘根目录位置
        /// </summary>
        public string? RootPath => _info.RootDirectory.FullName;

        /// <summary>
        /// 获取本地所有磁盘信息
        /// </summary>
        /// <returns></returns>
        public static DiskInfo[] GetDisks()
        {
            return DriveInfo.GetDrives().Select(x => new DiskInfo(x)).ToArray();
        }

        /// <summary>
        /// 获取 Docker 运行的容器其容器文件系统在主机中的存储位置
        /// </summary>
        /// <remarks>程序需要在宿主机运行才有效果，在容器中运行，调用此API获取不到相关信息</remarks>
        /// <returns></returns>
        public static DiskInfo[] GetDockerMerge()
        {
            return DriveInfo.GetDrives()
                 .Where(x => x.DriveFormat.Equals("overlay", StringComparison.OrdinalIgnoreCase) && x.DriveFormat.Contains("docker"))
                 .Select(x => new DiskInfo(x)).ToArray();
        }


        /// <summary>
        /// 筛选出真正能够使用的磁盘
        /// </summary>
        /// <returns></returns>
        public static DiskInfo[] GetRealDisk()
        {
            var disks = DriveInfo.GetDrives()
            .Where(x =>
            x.DriveType == DriveType.Fixed &&
            x.TotalSize != 0 && x.DriveFormat != "overlay");

            return disks.Select(x => new DiskInfo(x))
                .Distinct(new DiskInfoEquality()).ToArray();
        }

        /// <summary>
        /// 筛选重复项
        /// </summary>
        private class DiskInfoEquality : IEqualityComparer<DiskInfo>
        {
            public bool Equals(DiskInfo? x, DiskInfo? y)
            {
                return x?.Id == y?.Id;
            }

            public int GetHashCode(DiskInfo obj)
            {
                return obj.Id.GetHashCode();
            }
        }
    }

}
