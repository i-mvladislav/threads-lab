using System.Diagnostics;

const int n = 100;

int[] numbers = Enumerable.Range(1, n).ToArray();

decimal totalSum = 0;

Stopwatch sw = new Stopwatch();
sw.Start();

int partitionCount = 5;
decimal[] partitionSums = new decimal[partitionCount];
Thread[] workerThreads = new Thread[partitionCount];
int chunkSize = n / partitionCount;

sw.Reset();
sw.Start();


for (int partitionIndex = 0; partitionIndex < partitionCount; partitionIndex++)
{
    int chunkStart = partitionIndex * chunkSize;
    int chunkEnd = (partitionIndex == partitionCount - 1)
        ? n
        : chunkStart + chunkSize;

    int localIndex = partitionIndex;

    workerThreads[localIndex] = new Thread(() =>
    {
        decimal sum = 0;
        for (int i = chunkStart; i < chunkEnd; i++)
        {
            sum += numbers[i];
        }
        partitionSums[localIndex] = sum;
    });
    
    workerThreads[localIndex].Start();
}

foreach (var thread in workerThreads)
{
    thread.Join();
}

totalSum = partitionSums.Sum();
sw.Stop();

Console.WriteLine($"ElapsedMilliseconds: {sw.ElapsedMilliseconds}");
Console.WriteLine($"totalSum: {totalSum:F0}");