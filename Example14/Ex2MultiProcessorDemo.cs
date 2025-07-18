namespace Example14;

public class Ex2MultiProcessorDemo
{
    private static AutoResetEvent taskSignal = new AutoResetEvent(false);
    private static string? command;

    public static void Run()
    {
        Console.WriteLine("Сервер запущен...");

        for (int i = 1; i <= 3; i++)
        {
            var thread = new Thread(ProcessTask)
            {
                Name = $"Processor-{i}"
            };
            thread.Start();
        }

        while (true)
        {
            command = Console.ReadLine();
            if (!string.Equals(command, "q", StringComparison.OrdinalIgnoreCase))
            {
                taskSignal.Set();
            }
            else
            {
                taskSignal.Dispose();
                break;
            }
        }
    }

    private static void ProcessTask()
    {
        while (true)
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} ожидает...");
            taskSignal.WaitOne();
            
            Console.WriteLine($"{Thread.CurrentThread.Name} выполняет задачу.");
            Thread.Sleep(5000);
        }
    }
}