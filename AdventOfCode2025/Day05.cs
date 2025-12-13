static class Day05
{
    internal static void Run(TextReader reader)
    {
        List<(ulong Lower, ulong Upper)> ranges = reader.GetRanges().OrderBy(r => r.Lower).ToList();
        int resultPart1 = reader.GetIngredientIDs().FindFresh(ranges).Count();
        Console.WriteLine($"Answer to Part 1 = {resultPart1}");
        ulong resultPart2 = ranges.MergeOverlaps().GetNumOfFresh().Sum();
        Console.WriteLine($"Answer to Part 2 = {resultPart2}");
    }

    private static IEnumerable<ulong> FindFresh(this IEnumerable<ulong> ids, IEnumerable<(ulong Lower, ulong Upper)> ranges) => ids
        .Where(id => ranges.Any(r => id.IsWithin(r)));

    private static bool IsWithin(this ulong ingredientID, (ulong Lower, ulong Upper) pair) =>
        pair.Lower <= ingredientID && pair.Upper >= ingredientID;

    private static IEnumerable<(ulong Lower, ulong Upper)> MergeOverlaps(this IEnumerable<(ulong Lower, ulong Upper)> ranges)
    {
        (ulong lower, ulong upper) = ranges.First();
        foreach ((ulong Lower, ulong Upper) in ranges.Skip(1))
        {
            if (upper >= Lower)
            {
                upper = Math.Max(upper, Upper);
            }
            else
            {
                yield return (lower, upper);
                (lower, upper) = (Lower, Upper);
            }
        }
        yield return (lower, upper);
    }

    private static IEnumerable<ulong> GetNumOfFresh(this IEnumerable<(ulong Lower, ulong Upper)> ranges) => ranges
        .Select(r => r.Upper - r.Lower + 1);

    private static IEnumerable<(ulong Lower, ulong Upper)> GetRanges(this TextReader reader) => reader
        .ReadLines()
        .TakeWhile(l => !string.IsNullOrEmpty(l))
        .Select(l => l.Split('-'))
        .Select(rs => (ulong.Parse(rs[0]), ulong.Parse(rs[1])));

    private static IEnumerable<ulong> GetIngredientIDs(this TextReader reader) => reader
        .ReadLines()
        .Select(ulong.Parse);
}
