namespace Example17;

public class Ex3CancellationTokenDemo
{
    public static void Run()
    {
        using CancellationTokenSource cts = new CancellationTokenSource();
        CancellationToken token = cts.Token;

        Thread worker = new Thread(() => PerformWork(token))
        {
            Name = "WorkerThread"
        };
        worker.Start();
        
        Console.WriteLine("Для отмены введите q");

        if (Console.ReadKey(intercept: false).KeyChar == 'q')
        {
            cts.Cancel();
        }

        worker.Join();
        Console.WriteLine("Главный поток завершён.");
    }

    private static void PerformWork(CancellationToken token)
    {
        Console.WriteLine("Рабочий поток начал выполнение.");

        while (true)
        {
            if (token.IsCancellationRequested)
            {
                Console.WriteLine("Отмена выполнена.");
                break;
            }

            Thread.Sleep(50);
            Console.Write("- ");
        }
        
        Console.WriteLine("\nРабочий поток завершил работу.");
    }
}