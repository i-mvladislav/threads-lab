namespace Example17;

public class Ex4CancellationTokenDemo
{
    public static void Run()
    {
        using var cts = new CancellationTokenSource();
        CancellationToken ct = cts.Token;

        Thread worker = new Thread(() =>
        {
            try
            {
                PerformWork(ct);
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"\n[Исключение]: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[Непредвиденная ошибка]: {ex}");
            }
            finally
            {
                Console.WriteLine("Рабочий поток завершает работу (finally).");
            }
        }) { Name = "WorkerThread" };

        worker.Start();
        
        Console.WriteLine("Для отмены введите q");
        if (Console.ReadKey(intercept: false).KeyChar == 'q')
        {
            cts.Cancel();
        }

        worker.Join();
        Console.WriteLine("Главный поток завершён.");
    }

    private static void PerformWork(CancellationToken ct)
    {
        Console.WriteLine("Рабочий поток начал выполнение.");

        while (true)
        {
            ct.ThrowIfCancellationRequested();
            
            Thread.Sleep(50);
            Console.Write("- ");
        }
    }
}