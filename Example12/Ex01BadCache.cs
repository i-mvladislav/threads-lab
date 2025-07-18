namespace Example12;

public class Ex01BadCache
{
    private Dictionary<int, string> cache = new();
    private object threadingLock = new();
    
    public void Add(int key, string value)
    {
        lock (threadingLock)
        {
            cache.Add(key, value);
        }
    }

    public string? Get(int key)
    {
        lock (threadingLock)
        {
            return cache.TryGetValue(key, out var result) ? result : null;
        }
    }
}