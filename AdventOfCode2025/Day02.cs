static class Day02
{
    internal static void Run(TextReader reader)
    {
        List<(long Lower, long Upper)> ranges = reader.GetRanges().ToList();
        long resultPart1 = ranges.Select(FindInvalidIDs).SelectMany(s => s).Sum();
        Console.WriteLine($"Answer to Part 1 = {resultPart1}");
    }

    private static long GetPower10(this int index) => (long)Math.Pow(10, index);

    private static int GetNumDigits(this long num) => (int)Math.Log10(num) + 1;

    private static bool HasEvenNumDigits(this long num) => num.GetNumDigits() % 2 == 0;

    private static long GetInvalidID(this long num) => num.GetFirstHalf() * ((num.GetNumDigits() / 2).GetPower10() + 1);

    private static long GetFirstHalf(this long num) => num / (num.GetNumDigits() / 2).GetPower10();

    private static long GetNextInvalidID(this long num) => (num + (num.GetNumDigits() / 2).GetPower10()).GetInvalidID();

    private static IEnumerable<long> GetFirstInvalidID(this (long lower, long upper) range)
    {
        long invalidId = range.lower.GetInvalidID();
        return (invalidId >= range.lower && invalidId <= range.upper) ? [invalidId] : [];
    }

    /// <summary>
    /// 4 cases:
    /// <para>- lower cannot > upper</para>
    /// <para>- lower with odd number of digits should be raised to the next number with even digits, aka 100000...</para>
    /// <para>- upper with odd number of digits should be reduced to the next number with even digits, aka 9999...</para>
    /// <para>- edge case of lower == upper</para>
    /// <para>- main logic: </para>
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    private static IEnumerable<long> FindInvalidIDs(this (long, long) range) => range switch
    {
        (long lower, long upper) when lower > upper => [],
        (long lower, long upper) when !lower.HasEvenNumDigits() => (lower.GetNumDigits().GetPower10(), upper).FindInvalidIDs(),
        (long lower, long upper) when !upper.HasEvenNumDigits() => (lower, (upper.GetNumDigits() - 1).GetPower10() - 1).FindInvalidIDs(),
        (long lower, long upper) when lower == upper => upper == upper.GetInvalidID() ? [upper] : [],
        (long lower, long upper) => range.GetFirstInvalidID().Concat((lower.GetNextInvalidID(), upper).FindInvalidIDs()),
    };

    private static IEnumerable<(long Lower, long Upper)> GetRanges(this TextReader reader) => reader.ReadToEnd()
        .Split(',')
        .Select(r => r.Split('-'))
        .Select(p => (long.Parse(p[0]), long.Parse(p[1])));
}