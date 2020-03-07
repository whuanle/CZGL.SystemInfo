

### 简介

CZGL.SystemInfo 是一个支持 Windows 和 Linux 的资源信息获取库，用于获取系统环境、机器资源信息、系统资源使用情况。

Nuget 搜索 `CZGL.SystemInfo` 即可安装。

类库中每一个属性和方法，我都加上了注释，调用时可以看得到。



## 平台通用

CZGL.SystemInfo 命名空间下，有个 EnvironmentInfo 静态类，用于获取各种信息。

CZGL.SystemInfo.Info 命名空间中，有三个类型，用于获取和记录不同类型的信息。



`MachineRunInfo` 用来获取机器运行使用的资源信息；

`SystemPlatformInfo` 用来获取系统平台信息；

`SystemRunEvnInfo` 获取系统属性信息；

`EnvironmentInfo.GetEnvironmentVariables()` 用于获取系统所有的环境变量。



### 获取某个属性信息

你可以这样使用

```c#
            // new实例获取
            MachineRunInfo m = new MachineRunInfo();
            Console.WriteLine("当前进程已用内存" + m.ThisUsedMem);
```



上面三个类型中，都有一个静态实例，也可以这样使用

```C#
            Console.WriteLine("当前进程已用内存" + MachineRunInfo.Instance.ThisUsedMem);
```



`MachineRunInfo` 、`SystemPlatformInfo`  、 `SystemRunEvnInfo`  三个类型，直接使用属性即可输出信息。



### 获取所有属性信息

如果你想一次性输出到控制台或者做一个统计，可以使用 EnvironmentInfo 中的方法来快速生成信息。

如果当前系统是中文，会输出中文备注。

```c#
            // 注意，一些资源的单位都是 kb

            // 获取系统平台信息
            KeyValuePair<string, object>[] a = env.GetSystemPlatformInfoValue();
            // 获取系统运行属性信息
            KeyValuePair<string, object>[] b = env.GetSystemRunInfoValue();
            // 获取机器资源信息
            KeyValuePair<string, object>[] c = env.GetMachineInfoValue();
            // 获取系统所有环境变量
            KeyValuePair<string, object>[] d = env.GetEnvironmentVariables();
```

打印示例

```c#
            Console.WriteLine("\n系统平台信息：\n");
            foreach (var item in a)
            {
                Console.WriteLine($"{item.Key}    :    {item.Value}");
            }

            Console.WriteLine("\n\n系统运行属性信息：\n");
            foreach (var item in b)
            {
                Console.WriteLine($"{item.Key}    :    {item.Value}");
            }

            Console.WriteLine("\n\n机器资源信息：\n");
            foreach (var item in c)
            {
                Console.WriteLine($"{item.Key}    :    {item.Value}");
            }

            Console.WriteLine("\n\n系统所有环境变量：\n");
            foreach (var item in d)
            {
                Console.WriteLine($"{item.Key}    :    {item.Value}");
            }
```

输出(部分显示)

```
系统平台信息：

运行框架    :    .NET Core 3.1.0
操作系统    :    Microsoft Windows 10.0.17763
操作系统版本    :    Microsoft Windows NT 6.2.9200.0
平台架构    :    X64


系统运行属性信息：

机器名称    :    aaaa-PC
当前关联用户名    :    aaa
用户网络域名    :    aaa-PC
系统已运行时间(毫秒)    :    3227500
Web程序核心框架版本    :    3.1.0
是否在交互模式中运行    :    True
分区磁盘    :    D:\, E:\, F:\, G:\, H:\, X:\
系统目录    :    X:\windows\system32


机器资源信息：

当前进程已使用物理内存    :    20020
当前进程已占耗CPU时间    :    328.125
系统所有进程各种使用的内存    :    System.Collections.Generic.KeyValuePair`2[System.String,System.Int64][]
系统已使用内存    :    5988340


系统所有环境变量：

VisualStudioVersion    :    16.0
CommonProgramFiles(x86)    :    x:\Program Files (x86)\Common Files
```



还可以使用 `(string, KeyValuePair<string, object>[]) GetMachineInfo()` 等，string 返回此类型信息的说明。



## Linux

Nuget 搜索 `CZGL.SystemInfo.Linux` 安装。

在这个库中，Linux 资源信息包括 进程计量，内存计量，CPU计量，虚拟内存计量，各种进程运行信息计量。



要通过实例化 `DynamicInfo` 才能获取。

有 5 个对象用于映射相应信息。

Tasks：用于统计进程数量，处于不同状态下的进程数。

CpuState：CPU 使用情况，CPU 各种负载信息。

Mem：物理内存和缓存使用情况。

Swap：虚拟内存使用情况。

PidInfo：一个进程的运行资源信息。

他们都有一个 IsSuccess 属性，用来判断是否能正常获取到 Linux 的信息。

实例化获取对象

```c#
            DynamicInfo info = new DynamicInfo();
```



### 直接使用

可以通过方法获取到相应的对象。

```c#
            var item = info.GetTasks();
            Console.WriteLine("系统中共有进程数    :" + item.Total);
            Console.WriteLine("正在运行的进程数    :" + item.Running);
```


###  批量获取

以下是批量获取的示例，每个属性和属性值生成一个键值对，可以批量获取信息列表。

如果当前系统是中文，会输出中文备注。

```c#
            // 获取进程统计
            KeyValuePair<string, object>[] a = info.GetRefTasks();

            // 获取CPU资源统计
            KeyValuePair<string, object>[] b = info.GetRefCpuState();

            // 获取内存统计
            KeyValuePair<string, object>[] c = info.GetRefMem();

            // 获取虚拟内存统计
            KeyValuePair<string, object>[] d = info.GetRefSwap();

            Dictionary<int, PidInfo> dic = info.GetPidInfo();

            Console.WriteLine("\n进程统计：\n");
            foreach (var item in a)
            {
                Console.WriteLine($"{item.Key}    :    {item.Value}");
            }

            Console.WriteLine("\n\nCPU资源统计：\n");
            foreach (var item in b)
            {
                Console.WriteLine($"{item.Key}    :    {item.Value}");
            }

            Console.WriteLine("\n\n内存统计：\n");
            foreach (var item in c)
            {
                Console.WriteLine($"{item.Key}    :    {item.Value}");
            }

            Console.WriteLine("\n\n获取虚拟内存统计：\n");
            foreach (var item in d)
            {
                Console.WriteLine($"{item.Key}    :    {item.Value}");
            }

            Console.WriteLine("\n\n 各个进程使用的资源：\n");
            Console.WriteLine("  进程Id  进程名称  所属用户    优化级  高低优先级  虚拟内存   物理内存   共享内存 进程状态  占用系统CPU(%)   占用内存(%d) ");

            foreach (var item in dic)
            {
                Console.WriteLine($"{item.Key}  {item.Value.Command}  {item.Value.User}  {item.Value.PR}  " +
                                  $"{item.Value.Nice}  {item.Value.VIRT}  {item.Value.RES}  {item.Value.SHR}  " +
                                  $"{item.Value.State}  {item.Value.CPU}  {item.Value.Mem}");
            }
        }

```

输出

```c#
进程统计：

Total    :    93
Running    :    1
Sleeping    :    59
Stopped    :    0
Zombie    :    0


CPU资源统计：

UserSpace    :    1
Sysctl    :    0.6
NI    :    0
Idolt    :    98.3
WaitIO    :    0.1
HardwareIRQ    :    0
SoftwareInterrupts    :    0


内存统计：

Total    :    1009048
Used    :    334040
Free    :    85408
Buffers    :    589600
CanUsed    :    675008


获取虚拟内存统计：

Total    :    0
Used    :    0
Free    :    0
AvailMem    :    505744

```



Windows 暂时不写了。剪头发去了。