static class Day03
{
    internal static void Run(TextReader reader)
    {
        List<int[]> banks = reader.GetBanks().ToList();
        int resultPart1 = banks.Select(GetLargestJoltage).Sum();
        Console.WriteLine($"Answer to Part 1 = {resultPart1}");
    }

    private static int ToNumber(this (int Left, int Right) pair) => pair.Left * 10 + pair.Right;

    private static int GetLargestJoltage(this int[] bank) => bank.Zip(bank.Skip(1))
        .Aggregate(Iterate)
        .ToNumber();

    private static (int, int) Iterate(this (int First, int Second) acc, (int First, int Second) curr) => (acc.First, acc.Second, curr.First, curr.Second) switch
    {
        (int accFirst, _, int currFirst, _) when currFirst > accFirst => curr,
        (int accFirst, int accSecond, _, int currSecond) when currSecond > accSecond => (accFirst, currSecond),
        _ => acc,
    };

    private static IEnumerable<int[]> GetBanks(this TextReader reader) => reader.ReadLines()
        .Select(b => b.Select(c => c - '0').ToArray());
}
