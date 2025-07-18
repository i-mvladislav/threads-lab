namespace Example14;

public class Ex3MultiProcessorManualResetDemo
{
    private static ManualResetEventSlim taskSignal = new ManualResetEventSlim(false);
    private static Queue<string> queue = new();
    static object queueLock = new();

    public static void Run()
    {
        Console.WriteLine("Сервер запущен...");

        for (int i = 1; i <= 5; i++)
        {
            new Thread(ProcessTask)
            {
                Name = $"Processor-{i}"
            }.Start();
        }

        while (true)
        {
            var command = Console.ReadLine();
            if (string.Equals(command, "q", StringComparison.OrdinalIgnoreCase))
            {
                taskSignal.Dispose();
                break;
            }

            lock (queueLock)
            {
                queue.Enqueue(command);
                taskSignal.Set();
            }
        }
    }

    private static void ProcessTask()
    {
        while (true)
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} ожидает...");
            taskSignal.Wait();

            while (true)
            {
                string cmd;
                lock (queueLock)
                {
                    if (queue.Count == 0)
                    {
                        taskSignal.Reset();
                        {
                            break;
                        }
                    }
                    cmd = queue.Dequeue();
                }
                
                Console.WriteLine($"{Thread.CurrentThread.Name} выполняет: {cmd}");
                Thread.Sleep(5000);
            }
        }
    }
}