static class Day06
{
    internal static void Run(TextReader reader)
    {
        string[] lines = reader.GetLines();
        ulong resultPart1 = lines.Calculate().Sum();
        Console.WriteLine($"Answer to Part 1 = {resultPart1}");
    }

    private static IEnumerable<ulong> Calculate(this string[] lines) => lines[0..^1]
        .SelectMany(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select((s, col) => (num: ulong.Parse(s), col)))
        .GroupBy(p => p.col, p => p.num)
        .OrderBy(g => g.Key)
        .Zip(lines[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries), (g, op) => g.Aggregate(op.GetOperation()));

    private static Func<ulong, ulong, ulong> GetOperation(this string operand) => operand switch
    {
        "+" => (a, b) => a + b,
        "*" => (a, b) => a * b,
        _ => throw new NotSupportedException(),
    };

    private static string[] GetLines(this TextReader reader) => reader.ReadLines().ToArray();
}
