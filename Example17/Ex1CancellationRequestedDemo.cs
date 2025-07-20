namespace Example17;

public class Ex1CancellationRequestedDemo
{
    private static bool cancellationRequested = false;

    public static void Run()
    {
        Thread workerThread = new Thread(PerformWork)
        {
            Name = "WorkerThread"
        };
        workerThread.Start();
        
        Console.WriteLine("Для отмены введите q");

        var input = Console.ReadKey(false).KeyChar;
        if (input == 'q')
        {
            cancellationRequested = true;
        }
        
        workerThread.Join();
    }

    private static void PerformWork()
    {
        Console.WriteLine("Рабочий поток начал выполнение.");

        while (true)
        {
            if (cancellationRequested)
            {
                Console.WriteLine("Отмена выполнена");
                break;
            }
            Thread.Sleep(50);
            Console.Write("- ");
        }
        
        Console.WriteLine("Рабочий поток завершил работу.");
    }
}