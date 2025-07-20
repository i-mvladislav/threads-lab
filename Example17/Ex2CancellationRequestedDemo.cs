namespace Example17;

public class Ex2CancellationRequestedDemo
{
    private static bool cancellationRequested = false;
    private static Exception? workerException;

    public static void Run()
    {
        Thread worker = new Thread(() =>
        {
            try
            {
                PerformWork();
            }
            catch (Exception ex)
            {
                workerException = ex;
            }
        }){ Name = "WorkerThread" };
        
        worker.Start();
        
        Console.WriteLine("Для отмены введите q");

        if (Console.ReadKey(false).KeyChar == 'q')
        {
            cancellationRequested = true;
        }
        
        worker.Join();

        if (workerException is not null)
        {
            Console.WriteLine("Ошибка в рабочем потоке: " + workerException.Message);
            throw new ApplicationException("Ошибка из потока", workerException);
        }
        else
        {
            Console.WriteLine("Рабочий поток завершился без ошибок");
        }
    }

    private static void PerformWork()
    {
        Console.WriteLine("Рабочий поток начал выполнение");
        while (true)
        {
            if (cancellationRequested)
            {
                throw new OperationCanceledException("Отменено");
            }
            
            Thread.Sleep(50);
            Console.Write("- ");
        }
    }
}