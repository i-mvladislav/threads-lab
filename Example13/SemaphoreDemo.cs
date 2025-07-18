namespace Example13;

public class SemaphoreDemo
{
    private static Queue<string?> pendingQueue = new();
    private static Semaphore concurrentLimiter = new(initialCount: 3, maximumCount: 3);
    private static object objLock = new();

    public static void Run()
    {
        Thread queueWatcher = new Thread(WatchQueue);
        queueWatcher.IsBackground = true;
        queueWatcher.Start();
        
        Console.WriteLine("Сервер запущен...");

        while (true)
        {
            var request = Console.ReadLine();
            if (request?.Equals("q", StringComparison.OrdinalIgnoreCase) == true)
            {
                break;
            }

            lock (objLock)
            {
                pendingQueue.Enqueue(request);
            }
        }
        concurrentLimiter.Dispose();
    }

    private static void WatchQueue()
    {
        while (true)
        {
            string? request = null;
            lock (objLock)
            {
                if (pendingQueue.Count > 0)
                {
                    request = pendingQueue.Dequeue();
                }
            }

            if (request is not null)
            {
                concurrentLimiter.WaitOne();
                Thread worker = new Thread(() => HandleRequest(request));
                worker.IsBackground = true;
                worker.Start();
            }
            
            Thread.Sleep(10);
        }
    }

    private static void HandleRequest(string? request)
    {
        try
        {
            Thread.Sleep(5000); // эмуляция обработки
            Console.WriteLine($"Обработан запрос: {request}");
        }
        finally
        {
            int previous = concurrentLimiter.Release();
            int current = previous + 1;
            Console.WriteLine(
                $"Поток {Thread.CurrentThread.ManagedThreadId} завершил работу, previous: {previous}"
            );
        }
    }
}