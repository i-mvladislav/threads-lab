namespace Example08;

public class Booking
{
    private bool _isRunning = true;
    private readonly Queue<string> _commands = new();
    private int _availableSeats = 5;
    private readonly Lock _availableSeatsLock = new();

    public void Processing()
    {
        Thread processingThread = new Thread(Observe);
        processingThread.Start();

        while (true)
        {
            string? command = Console.ReadLine();
            if (command?.ToLower() == "q")
            {
                _isRunning = false;
                break;
            }
            
            Monitor.Enter(_commands);

            try
            {
                _commands.Enqueue(command);
            }
            finally
            {
                Monitor.Exit(_commands);
            }
        }
        
        processingThread.Join();
    }
    
    public void Observe()
    {
        while (_isRunning)
        {
            string? command = null!;
            Monitor.Enter(_commands);
            try
            {
                _commands.TryDequeue(out command);
            }
            finally
            {
                Monitor.Exit(_commands);
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
                _isRunning = false;
                return;
        }
    }
    
    public void Book()
    {
        Monitor.Enter(_availableSeatsLock);
        try
        {
            if (_availableSeats == 0)
            {
                Console.WriteLine("The carriage is full");
            }
            else
            {
                _availableSeats--;
                Console.WriteLine($"Available seats: {_availableSeats}");
            }
        }
        finally
        {
            Monitor.Exit(_availableSeatsLock);
        }
    }

    public void CancelReservation()
    {
        Monitor.Enter(_availableSeatsLock);

        try
        {
            if (_availableSeats < 5)
            {
                _availableSeats++;
                Console.WriteLine($"Available seats: {_availableSeats}");
            }
            else
            {
                Console.WriteLine($"Operation canceled");
            }
        }
        finally
        {
            Monitor.Exit(_availableSeatsLock);
        }
    }
}