namespace Example15;

public class Ex2ConfigLogDeadlockDemo
{
    private static object configLock = new object();
    private static object logLock = new object();
    private static Thread[] threads = new Thread[2];

    public static void Run()
    {
        Thread saveConfigThread = new Thread(SaveConfig) { Name = "ConfigSaver" };
        
        threads[0] = Thread.CurrentThread;
        threads[0].Name = "MainThread";
        threads[1] = saveConfigThread;

        new Thread(PrintThreadState) { IsBackground = true }.Start();
        
        saveConfigThread.Start();
        WriteLog();

        saveConfigThread.Join();
        Console.WriteLine("Готово.");
    }

    private static void SaveConfig()
    {
        lock (configLock)
        {
            Console.WriteLine($"ConfigSaver: захватил configLock");
            Thread.Sleep(1000);

            lock (logLock)
            {
                Console.WriteLine("ConfigSaver: захватил loglock");
            }
        }
    }
    
    private static void PrintThreadState()
    {
        while (true)
        {
            Thread.Sleep(990);
            Console.WriteLine($"---- {DateTime.Now:HH:mm:ss} ----");
            foreach (var thread in threads)
            {
                Console.WriteLine($"{thread.Name}: {thread.ThreadState}");
            }

            Console.WriteLine();
        }
    }

    private static void WriteLog()
    {
        lock (logLock)
        {
            Console.WriteLine($"LogThread: захватил logLock");
            Thread.Sleep(1500);

            lock (configLock)
            {
                Console.WriteLine("LogThread: захватил configLock");
            }
        }
    }
}