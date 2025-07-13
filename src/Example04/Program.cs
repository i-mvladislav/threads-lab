using System.Diagnostics;

const int n = 1_000_000_000;

int[] numbers = Enumerable.Range(1, n).ToArray();

decimal totalSum = 0;

Stopwatch sw = new Stopwatch();
sw.Start();

for (int i = 0; i < n; i++)
{
    totalSum += numbers[i];
}

sw.Stop();

Console.WriteLine($"ElapsedMilliseconds: {sw.ElapsedMilliseconds}");
Console.WriteLine($"Total sum: {totalSum:F0}");

// ===

sw.Reset();
sw.Start();
totalSum = 0;
var chunks = numbers.Chunk(n / 5).ToArray();
var threads = new Thread[5];
for (int i = 0; i < threads.Length; i++)
{
    var chunkNum = i;
    threads[i] = new Thread(() =>
    {
        for(int b = 0; b < chunks[chunkNum].Length; b++)
        {
            totalSum += chunks[chunkNum][b];
        }
    });
}

foreach (var thread in threads)
{
    thread.Start();
    thread.Join();
}

sw.Stop();
Console.WriteLine($"ElapsedMilliseconds: {sw.ElapsedMilliseconds}");
Console.WriteLine($"Total sum: {totalSum:F0}");
