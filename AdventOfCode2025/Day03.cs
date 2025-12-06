static class Day03
{
    internal static void Run(TextReader reader)
    {
        List<byte[]> banks = reader.GetBanks().ToList();
        ulong resultPart1 = banks.Select(bank => bank.GetLargestJoltageWith(2)).Sum();
        Console.WriteLine($"Answer to Part 1 = {resultPart1}");
        ulong resultPart2 = banks.Select(bank => bank.GetLargestJoltageWith(12)).Sum();
        Console.WriteLine($"Answer to Part 2 = {resultPart2}");
    }

    private static ulong ToNumber(this IEnumerable<byte> batteries) => batteries.Aggregate(0UL, (acc, curr) => acc * 10UL + curr);

    /// <summary>
    /// Return index of battery before the first ascent in sequence,
    /// use MaxValue for comparison with last element on the list to guarantee returning index of last element
    /// </summary>
    /// <param name="batteries"></param>
    /// <returns></returns>
    private static int IndexToDrop(this byte[] batteries) => batteries
        .Zip([.. batteries.Skip(1), byte.MaxValue], Enumerable.Range(0, batteries.Length))
        .First(t => t.First < t.Second)
        .Third;

    private static ulong GetLargestJoltageWith(this byte[] batteries, int numBatteries) => batteries.Length == numBatteries
        ? batteries.ToNumber()
        : batteries
            .Where((_, idx) => idx != batteries.IndexToDrop())
            .ToArray()
            .GetLargestJoltageWith(numBatteries);

    private static IEnumerable<byte[]> GetBanks(this TextReader reader) => reader.ReadLines()
        .Select(b => b.Select(c => (byte)(c - '0')).ToArray());


    // ==================== Below are a previous solution before converting fully to functional code ====================

    //private static ulong GetLargestJoltagePart1(this byte[] batteries) => batteries.Zip(batteries.Skip(1), (first, second) => new byte[] { first, second })
    //    .Aggregate(IteratePart1)
    //    .ToNumber();

    //private static byte[] IteratePart1(this byte[] acc, byte[] curr) => (acc, curr) switch
    //{
    //    ([byte accFirst, _], [byte currFirst, _]) when currFirst > accFirst => curr,
    //    ([byte accFirst, byte accSecond], [_, byte currSecond]) when currSecond > accSecond => [accFirst, currSecond],
    //    _ => acc,
    //};

    //private static ulong OldGetLargestJoltageWith(this byte[] batteries, int numBatteries) => batteries.Skip(numBatteries)
    //   .Aggregate(batteries.Take(numBatteries).ToArray(), ChooseBatteries)
    //   .ToNumber();

    //private static byte[] ChooseBatteries(this byte[] currentBatteries, byte nextBattery) => currentBatteries
    //    .AddToEnd(nextBattery)
    //    .DropOne();

    //private static byte[] AddToEnd(this byte[] batteries, byte nextBattery) => [.. batteries, nextBattery];

    //private static byte[] DropOne(this byte[] batteries) => batteries
    //    .Where((b, idx) => idx != batteries.IndexToDrop())
    //    .ToArray();

    //private static int IndexToDropOld(this byte[] batteries)
    //{
    //    for (int i = 0; i < batteries.Length - 1; ++i)
    //    {
    //        if (batteries[i] < batteries[i + 1])
    //            return i;
    //    }
    //    return batteries.Length - 1;
    //}
}
