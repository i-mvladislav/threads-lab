namespace Example12;

public class Ex02ConfigCache
{
    private Dictionary<int, string> cache = new();
    private ReaderWriterLockSlim objLock = new();

    public void Add(int id, string data)
    {
        try
        {
            objLock.EnterWriteLock();
            cache[id] = data;
        }
        finally
        {
            objLock.ExitWriteLock();
        }
    }

    public string? Get(int id)
    {
        try
        {
            objLock.EnterReadLock();
            return cache.TryGetValue(id, out var data) ? data : null;
        }
        finally
        {
            objLock.ExitReadLock();
        }
    }
}