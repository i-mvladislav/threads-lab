namespace Example08;

public class Booking
{
    private bool isRunning = true;
    private readonly Queue<string> commands = new();
    private int availableSeats = 5;
    private readonly Lock availableSeatsLock = new();

    public void Processing()
    {
        Thread processingThread = new Thread(Monitor);
        processingThread.Start();

        while (true)
        {
            string? command = Console.ReadLine();
            if (command?.ToLower() == "q")
            {
                isRunning = false;
                break;
            }

            lock (commands)
            {
                commands.Enqueue(command);
            }
        }
        
        processingThread.Join();
    }
    
    public void Monitor()
    {
        while (isRunning)
        {
            string? command = null!;
            lock (commands)
            {
                commands.TryDequeue(out command);
            }

            if (command is not null)
            {
                new Thread(() => Execute(command)).Start();
            }
            else
            {
                Thread.Sleep(10);
            }
        }
    }

    public void Execute(string input)
    {
        switch (input)
        {
            case "1":
                Thread.Sleep(2000);
                Book();
                break;
            case "2":
                Thread.Sleep(2000);
                CancelReservation();
                break;
            case "q":
                isRunning = false;
                return;
        }
    }
    
    public void Book()
    {
        lock (availableSeatsLock)
        {
            if (availableSeats == 0)
            {
                Console.WriteLine("The carriage is full");
            }
            else
            {
                availableSeats--;
                Console.WriteLine($"Available seats: {availableSeats}");
            }
        }
    }

    public void CancelReservation()
    {
        lock (availableSeatsLock)
        {
            if (availableSeats < 5)
            {
                availableSeats++;
                Console.WriteLine($"Available seats: {availableSeats}");
            }
            else
            {
                Console.WriteLine($"Operation canceled");
            }
        }
    }
}