namespace Example14;

public class Ex1SingleProcessorDemo
{
    private static AutoResetEvent taskSignal = new AutoResetEvent(false);
    private static string? command;

    public static void Run()
    {
        var thread = new Thread(ProcessTask)
        {
            Name = "SingleProcessor "
        };
        thread.Start();
        
        Console.WriteLine("Обработчик запущен");

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
            Console.WriteLine($"{Thread.CurrentThread.Name} ожидает сигнала.");
            taskSignal.WaitOne();
            
            Console.WriteLine($"{Thread.CurrentThread.Name} выполняет задачу.");
            Thread.Sleep(2000);
        }
    }
}