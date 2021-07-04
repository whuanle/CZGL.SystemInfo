using CZGL.SystemInfo;
using System;
using Xunit;


namespace InfoTests
{
    public class PeocessInfoTests
    {
        [Fact]
        public void A()
        {
            foreach(var item in ProcessInfo.GetProcessList())
            {
                var tmp = ProcessInfo.GetProcess(item.Key);

            }
        }
    }
}
