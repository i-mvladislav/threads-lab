void PrintThreadIds()
{
    int remainingIterations = 100;
    while (remainingIterations-- > 0)
    {
        Thread currentThread = Thread.CurrentThread;
        Console.Write(currentThread.Name + " ");
        Thread.Sleep(10);
    }
}

int count = 10;

Thread[] threads = new Thread[count];

for (int i = 0; i < count; i++)
{
    Thread thread = threads[i];
    thread = new Thread(PrintThreadIds);
    if (i < 5)
    {
        thread.Priority = ThreadPriority.Lowest;
        thread.Name = "L";
    }
    else
    {
        thread.Priority = ThreadPriority.Highest;
        thread.Name = "H";
    }
    thread.Start();
}