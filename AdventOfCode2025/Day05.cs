static class Day05
{
    internal static void Run(TextReader reader)
    {
        List<(ulong Lower, ulong Upper)> ranges = reader.GetRanges().ToList();
        List<ulong> ingredientIDs = reader.GetIngredientIDs().ToList();
        int resultPart1 = ingredientIDs.FindFresh(ranges).Count();
        Console.WriteLine($"Answer to Part 1 = {resultPart1}");
    }

    private static IEnumerable<ulong> FindFresh(this IEnumerable<ulong> ids, IEnumerable<(ulong Lower, ulong Upper)> ranges) => ids
        .Where(id => ranges.Any(r => id.IsWithin(r)));

    private static bool IsWithin(this ulong ingredientID, (ulong Lower, ulong Upper) pair) =>
        pair.Lower <= ingredientID && pair.Upper >= ingredientID;

    private static IEnumerable<(ulong Lower, ulong Upper)> GetRanges(this TextReader reader) => reader
        .ReadLines()
        .TakeWhile(l => !string.IsNullOrEmpty(l))
        .Select(l => l.Split('-'))
        .Select(rs => (ulong.Parse(rs[0]), ulong.Parse(rs[1])));

    private static IEnumerable<ulong> GetIngredientIDs(this TextReader reader) => reader
        .ReadLines()
        .Select(ulong.Parse);
}
