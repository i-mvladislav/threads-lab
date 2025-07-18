string fileName = "count.txt";

string mutexName = $"Global\\Mutex.{fileName}";

using var mutex = new Mutex(false, mutexName);

for (int i = 0; i < 100_000; i++)
{
    mutex.WaitOne();
    try
    {
        int counter = GetCount(fileName);
        counter++;
        UpdateCount(fileName, counter);
    }
    finally
    {
        mutex.ReleaseMutex();
    }
}

Console.WriteLine("Готово");

static int GetCount(string path)
{
    if (!File.Exists(path))
    {
        return 0;
    }

    using var stream = new FileStream(
        path,
        FileMode.Open,
        FileAccess.Read,
        FileShare.ReadWrite
    );
    
    using var reader = new StreamReader(stream);

    string? content = reader.ReadToEnd();
    return string.IsNullOrWhiteSpace(content) ? 0 : int.Parse(content);
}

static void UpdateCount(string path, int counter)
{
    using var stream = new FileStream(
        path,
        FileMode.Create,
        FileAccess.Write,
        FileShare.ReadWrite
    );
    
    using var writer = new StreamWriter(stream);
    writer.Write(counter);
}