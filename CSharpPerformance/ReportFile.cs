using System.Text;

namespace CSharpPerformance;
internal static class ReportFile
{
    public static void WriteResultCsv(string className, List<string> FileSource)
    {
        FileSource.Insert(0, "Count,EagerEvaluate (ms),LazyEvaluate(ms),EEIncrementalMemory(KB),LEIncrementalMemory(KB)");
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmm");
        var fileName = $"{className}_results_{timestamp}.csv";
        System.IO.File.WriteAllLines(fileName, FileSource);

        StringBuilder sbInfo = EnviromentInfo.Get();
        sbInfo.AppendLine($"\nCreated Result Name:{fileName}");
        System.IO.File.WriteAllText($"PerformanceMeasurement{timestamp}.log", sbInfo.ToString());
    }
}
