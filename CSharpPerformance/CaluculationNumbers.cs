using System.Diagnostics;

namespace CSharpPerformance;

internal class CaluculationNumbers
{
    public void PerformAndWriteResultCsv()
    {
        var results = PerformEvaluation();
        WriteResultsToCsv(results);
    }

    private List<(int count, double eagerTime, double lazyTime, double eagerMemory, double lazyMemory)> PerformEvaluation()
    {
        int[] counts = [1000000, 500000, 100000, 50000, 10000, 5000, 1000];
        var iterations = 10;

        var results = new List<(int count, double eagerTime, double lazyTime, double eagerMemory, double lazyMemory)>();

        foreach (int count in counts)
        {
            var eagerTimes = new List<double>();
            var lazyTimes = new List<double>();
            var eagerMemory = new List<double>();
            var lazyMemory = new List<double>();

            for (int i = 0; i < iterations; i++)
            {
                (double time, long memory) = Evaluate(count, true);
                eagerTimes.Add(time);
                eagerMemory.Add(memory / 1024.0);

                (time, memory) = Evaluate(count, false);
                lazyTimes.Add(time);
                lazyMemory.Add(memory / 1024.0);
            }
            results.Add((count, eagerTimes.Average(), lazyTimes.Average(), eagerMemory.Average(), lazyMemory.Average()));
        }

        return results;
    }

    private (double time, long memory) Evaluate(int count, bool eager)
    {
        var memoryBefore = GC.GetTotalMemory(true);
        var stopwatch = Stopwatch.StartNew();
        var numbers = eager ? Enumerable.Range(1, count).ToList() : Enumerable.Range(1, count);
        var result = numbers.Select(number => (number / 2 * 2 / 2) + 1); //ここでは計算処理をするだけ
        stopwatch.Stop();
        var memoryAfter = GC.GetTotalMemory(true);
        return (stopwatch.Elapsed.TotalMilliseconds, memoryAfter - memoryBefore);
    }

    private void WriteResultsToCsv(List<(int count, double eagerTime, double lazyTime, double eagerMemory, double lazyMemory)> results)
    {
        var csv = new List<string>();

        foreach (var result in results)
        {
            string line = $"{result.count},{result.eagerTime:0.000},{result.lazyTime:0.000},{result.eagerMemory:0.000},{result.lazyMemory:0.000}";
            csv.Add(line);
        }

        ReportFile.WriteResultCsv(GetType().Name, csv);
    }

}