using System.Text.RegularExpressions;

static class Day06
{
    internal static void Run(TextReader reader)
    {
        string[] lines = reader.GetLines();
        ulong resultPart1 = lines.CalculateVertical().Sum();
        Console.WriteLine($"Answer to Part 1 = {resultPart1}");
        ulong resultPart2 = lines.CalculateHorizontal().Sum();
        Console.WriteLine($"Answer to Part 2 = {resultPart2}");
    }

    private static IEnumerable<ulong> CalculateVertical(this string[] lines) => lines[0..^1]
        .SelectMany(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select((s, col) => (num: ulong.Parse(s), col)))
        .GroupBy(p => p.col, p => p.num)
        .OrderBy(g => g.Key)
        .Zip(lines[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries),
            (g, op) => g.Aggregate(op.GetOperation()));

    private static Func<ulong, ulong, ulong> GetOperation(this string operand) => operand switch
    {
        "+" => (a, b) => a + b,
        "*" => (a, b) => a * b,
        _ => throw new NotSupportedException(),
    };

    private static IEnumerable<ulong> CalculateHorizontal(this string[] lines) => lines[0..^1]
        .SelectMany(line => line.SplitNums(lines[^1].SplitOperands()).Select((num, col) => (num, col)))
        .GroupBy(p => p.col, p => p.num)
        .OrderBy(g => g.Key)
        .Select(ReadCephalopod)
        .Zip(lines[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries),
            (g, op) => g.Aggregate(op.GetOperation()));

    private static IEnumerable<string> SplitNums(this string nums, IEnumerable<string> operands) => Regex
        .Matches(nums, string.Join(@"\s", operands.Select(o => @$"(.{{{o.Length}}})")))
        .Select(m => m.Groups)
        .SelectMany(gc => gc.Values.Skip(1))
        .Select(g => g.Value);

    private static IEnumerable<string> SplitOperands(this string operands) => Regex
        .Matches(operands + " ", @"([+*]\s+)+")
        .SelectMany(m => m.Groups[1].Captures)
        .Select(c => c.Value[0..^1]);

    private static IEnumerable<ulong> ReadCephalopod(this IEnumerable<string> column) => column
        .SelectMany((num, row) => num.Select((c, col) => (c, row, col)))
        .GroupBy(t => t.col, t => (t.c, t.row))
        .Select(g => g.OrderBy(t => t.row).Select(t => t.c).Collect());

    private static ulong Collect(this IEnumerable<char> digits) => ulong.Parse(string.Join("", digits));

    private static string[] GetLines(this TextReader reader) => reader.ReadLines().ToArray();
}
