using CZGL.SystemInfo;
using System;
using Xunit;

namespace InfoTests
{
    public class RuntimeInfoTests
    {
        [Fact]
        public void CheckTests()
        {
            var type = typeof(SystemPlatformInfo);

            foreach (var item in type.GetProperties())
            {
                var value = item.GetValue(null);
                Assert.NotNull(value);
            }
        }
    }
}
