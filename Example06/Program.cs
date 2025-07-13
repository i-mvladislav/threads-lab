bool running = true;
Queue<string?> requests = new();

Thread monitorThread = new(MonitorQueue);
monitorThread.Start();
Console.WriteLine("Веб-сервер запущен: ");

while (running)
{
    Console.Write("Запрос: ");
    string? request = Console.ReadLine();
    if (request?.ToLower() == "q")
    {
        running = false;
        break;
    }
    requests.Enqueue(request);
}

monitorThread.Join();

void MonitorQueue()
{
    while (running)
    {
        if (requests.TryDequeue(out var request) && request is not null)
        {
            new Thread(() => ProcessRequest(request)).Start();
        }
        else
        {
            Thread.Sleep(50);
        }
    }
}

void ProcessRequest(string? request)
{
    Thread.Sleep(3000); // Имитация времени обработки запроса
    Console.WriteLine($"\nОбработан запрос: '{request}'...");
}