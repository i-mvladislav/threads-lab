namespace Example13;

public class SemaphoreSlimDemo
{
    private static Queue<string?> pendingQueue = new();
    private static SemaphoreSlim concurrentLimiter = new(initialCount: 3, maxCount: 3);
    private static object objLock = new();

    public static void Run()
    {
        new Thread(GetInfo) { IsBackground = true }.Start();
        Thread.Sleep(2000);

        Thread queueWatcher = new(WatchQueue);
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

    static void WatchQueue()
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

                if (request is not null)
                {
                    concurrentLimiter.Wait();
                    Thread worker = new Thread(() => HandleRequest(request));
                    worker.Start();
                }
                Thread.Sleep(100);
            }
        }
    }

    private static void HandleRequest(string? request)
    {
        try
        {
            Thread.Sleep(5000);
            Console.WriteLine($"Обработан запрос: {request}");
        }
        finally
        {
            int previous = concurrentLimiter.Release();
            int current = concurrentLimiter.CurrentCount;
            string text = string.Format(
                "Поток: {0} завершил работу, previous семафора: {1}, current: {2}",
                Thread.CurrentThread.ManagedThreadId,
                previous,
                current
            );
            Console.WriteLine(text);
        }
    }

    private static void GetInfo()
    {
        while (true)
        {
            string text = string.Format(
                "Поток: {0}, current: {1}",
                Thread.CurrentThread.ManagedThreadId,
                concurrentLimiter.CurrentCount
            );
            Console.WriteLine(text);
            Thread.Sleep(1000);
        }
    }
}