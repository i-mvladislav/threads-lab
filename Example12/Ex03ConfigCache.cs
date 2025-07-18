namespace Example12;

public class Ex03ConfigCache
{
    private Dictionary<int, string> cache = new();
    private ReaderWriterLockSlim objLock = new();

    public void Add(int id, string data)
    {
        bool taken = false;
        try
        {
            objLock.EnterWriteLock();
            taken = true;
            cache[id] = data;
        }
        finally
        {
            if (taken)
            {
                objLock.ExitWriteLock();
            }
        }
    }

    public string? Get(int id)
    {
        bool taken = false;
        try
        {
            objLock.EnterReadLock();
            taken = true;
            return cache.TryGetValue(id, out var data) ? data : null;
        }
        finally
        {
            if (taken)
            {
                objLock.ExitReadLock();
            }
        }
    }
}