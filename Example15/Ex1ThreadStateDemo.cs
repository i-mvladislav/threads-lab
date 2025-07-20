namespace Example15;

public class Ex1ThreadStateDemo
{
    private static Thread[] workerThreads = new Thread[3];

    public static void Run()
    {
        for (int i = 0; i < workerThreads.Length; i++)
        {
            workerThreads[i] = new Thread(PerformWork)
            {
                Name = $"Worker {i}"
            };
            
            Console.WriteLine($"{workerThreads[i].Name} state: {workerThreads[i].ThreadState}");
            workerThreads[i].Start();
        }
        
        Thread.Sleep(3000);
        foreach (var thread in workerThreads)
        {
            Console.WriteLine($"{thread.Name} state: {thread.ThreadState}");
        }

        Thread.CurrentThread.Name = "Master";
        PerformWork();
        
        Thread.Sleep(3000);
        foreach (var thread in workerThreads)
        {
            Console.WriteLine($"{thread.Name} state: {thread.ThreadState}");
        }
    }

    private static void PerformWork()
    {
        var th = Thread.CurrentThread;
        Console.WriteLine($"{th.Name} started. State = {th.ThreadState}");
        Thread.Sleep(10000);
        Console.WriteLine($"{th.Name} finished. State = {th.ThreadState}");
    }
}