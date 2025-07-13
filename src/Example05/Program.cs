using System.Diagnostics;

const int rowsCount = 2;
const int colsCount = 3;

Stopwatch stopwatch = new Stopwatch();
stopwatch.Start();

var matrixA = new int[rowsCount, colsCount];
var matrixB = new int[rowsCount, colsCount];
var matrixC = new int[rowsCount, colsCount];

FillMatrix(matrixA);
FillMatrix(matrixB);

PrintMatrix(matrixA);
PrintMatrix(matrixB);

var workerThreads = new Thread[rowsCount];

for (var i = 0; i < workerThreads.Length; i++)
{
    var row = i;
    workerThreads[i] = new Thread(() =>
    {
        for (var col = 0; col < colsCount; col++)
        {
            matrixC[row, col] = matrixA[row, col] + matrixB[row, col];
        }
    });
}

foreach (var thread in workerThreads)
{
    thread.Start();
    thread.Join();
}

PrintMatrix(matrixC);

stopwatch.Stop();
Console.WriteLine($"ElapsedMilliseconds: {stopwatch.ElapsedMilliseconds}");

void FillMatrix(int[,] matrix)
{
    for (var row = 0; row < rowsCount; row++)
    {
        for (var col = 0; col < colsCount; col++)
        {
            matrix[row, col] = Random.Shared.Next(1, 10);
        }
    }
}

void PrintMatrix(int[,] matrix)
{
    Console.WriteLine("Matrix --");
    for (var row = 0; row < rowsCount; row++)
    {
        for (var col = 0; col < colsCount; col++)
        {
            Console.Write(matrix[row, col] + " ");
        }
        Console.WriteLine();
    }
    
    Console.WriteLine();
}