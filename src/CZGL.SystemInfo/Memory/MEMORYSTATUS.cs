namespace CZGL.SystemInfo
{
    /// <summary>
    /// 
    /// </summary>
    public struct MEMORYSTATUS
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public uint dwTotalPhys;
        public uint dwAvailPhys;
        public uint dwTotalPageFile;
        public uint dwAvailPageFile;
        public uint dwTotalVirtual;
        public uint dwAvailVirtual;
    }
}
