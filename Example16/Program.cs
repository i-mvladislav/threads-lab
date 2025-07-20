int result = 0;

Thread worker = new Thread(() =>
{
   result = ComputeValue(10, 20);
});

worker.Start();
//worker.Join();

Console.WriteLine($"Результат: {result}");

int ComputeValue(int a, int b)
{
   Thread.Sleep(2000);
   return a + b;
}