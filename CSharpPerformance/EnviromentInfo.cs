using System.Management;
using System.Runtime.InteropServices;
using System.Text;

namespace CSharpPerformance;
internal static class EnviromentInfo
{
    public static StringBuilder Get()
    {
        var sb = new StringBuilder();
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return sb.AppendLine("CPU and memory information cannot be obtained in non-Windows environments");

        sb.AppendLine("Enviroment Information");
        ManagementObjectSearcher cpuSearcher = new("SELECT * FROM Win32_Processor");
        foreach (ManagementObject cpuInfo in cpuSearcher.Get().Cast<ManagementObject>())
        {
            sb.AppendLine($"CPU Name: {cpuInfo["Name"]}");
            sb.AppendLine($"CPU Clock Speed: {cpuInfo["MaxClockSpeed"]} MHz");
            sb.AppendLine($"CPU Number of Cores: {cpuInfo["NumberOfCores"]}");
            sb.AppendLine($"CPU Number of Logical Processors: {cpuInfo["NumberOfLogicalProcessors"]}");
        }

        ManagementObjectSearcher memorySearcher = new("SELECT * FROM Win32_PhysicalMemory");
        long totalMemoryBytes = 0;
        foreach (ManagementObject memoryInfo in memorySearcher.Get().Cast<ManagementObject>())
        {
            totalMemoryBytes += Convert.ToInt64(memoryInfo["Capacity"]);
        }
        sb.AppendLine($"Total Memory: {totalMemoryBytes / (1024 * 1024 * 1024)} GB");
        sb.AppendLine($".NET Version: {RuntimeInformation.FrameworkDescription}");
        return sb;
    }

}

