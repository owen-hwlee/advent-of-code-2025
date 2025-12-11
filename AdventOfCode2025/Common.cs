static class Common
{
    public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
    {
        foreach (T item in sequence)
            action(item);
    }

    public static IEnumerable<T> Do<T>(this IEnumerable<T> sequence, Action<T> action)
    {
        foreach (T item in sequence)
        {
            action(item);
            yield return item;
        }
    }

    public static IEnumerable<string> ReadLines(this TextReader reader)
    {
        while (reader.ReadLine() is string line)
        {
            yield return line;
        }
    }

    public static ulong Sum(this IEnumerable<ulong> ulongs) => ulongs.Aggregate(0UL, (acc, x) => acc + x);
}