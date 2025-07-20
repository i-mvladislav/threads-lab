namespace Example15;

public class Ex3OtherWaitSleep
{
    private static List<Thread> threads = [];

    public static void Run()
    {
        new Thread(PrintThreadState) { IsBackground = true }.Start();
        Thread thread = new Thread(() =>
        {
            Console.WriteLine(" >>> Thread.Sleep...");
            Thread.Sleep(2000); // освобождает ресурсы системы
            
            Console.WriteLine(" >>> Thread.SpinWait... ");
            Thread.SpinWait(2_000_000_000); // while(c-- > 0) { } - имитация пустого цикла, статус треда Running, ресурсы не освобождает
            Thread.Sleep(2000);
            
            Console.WriteLine(" >>> Thread.SpinUntil start... ");
            // Если дольше 4000 мс, ожидание будет закончено либо закончено если операция выполнилась быстрее
            SpinWait.SpinUntil(() =>
            {
                Thread.Sleep(5000);
                return true;
            }, 4000);
            Console.WriteLine(" >>> Thread.SpinUntil end... ");
        });
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
}