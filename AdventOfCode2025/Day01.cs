static class Day01
{
    internal static void Run(TextReader reader)
    {
        List<int> rotations = reader.LoadMoves().ToList();
        int resultPart1 = rotations.StartPassAt(50, Part.Part1).Count;
        int resultPart2 = rotations.StartPassAt(50, Part.Part2).Count;
        Console.WriteLine($"Answer to Part 1 = {resultPart1}");
        Console.WriteLine($"Answer to Part 2 = {resultPart2}");
    }

    private static (int Position, int Count) StartPassAt(this IEnumerable<int> rotations, int initialValue, Part part) =>
        rotations.Aggregate((initialValue, 0),
            ((int, int) state, int rotation) => state.Move(rotation, part));

    private static (int Position, int Count) Move(this (int position, int count) state, int rotation, Part part)
    {
        int newPosition = state.position.Apply(rotation);
        int numHits = part switch
        {
            Part.Part1 => newPosition.PointsAt0() ? 1 : 0,
            Part.Part2 => state.position.CountPassesOf0By(rotation),
            _ => throw new NotSupportedException(),
        };
        return (newPosition, state.count + numHits);
    }

    private static int Apply(this int position, int rotation) => (((position + rotation) % 100) + 100) % 100;

    private static bool PointsAt0(this int position) => position == 0;

    private static int CountPassesOf0By(this int position, int rotation) => (position + rotation) switch
    {
        _ when position == 0 => Math.Abs(rotation / 100),
        > 0 and < 100 => 0,
        int gross => Math.Abs(gross / 100) + (gross <= 0 ? 1 : 0),
    };

    private static IEnumerable<int> LoadMoves(this TextReader reader) => reader.ReadLines().Select(ParseMove);

    private static int ParseMove(this string move) => move switch
    {
        ['L', .. string num] => -int.Parse(num),
        ['R', .. string num] => int.Parse(num),
        _ => throw new NotSupportedException(),
    };

    private enum Part
    {
        Part1,
        Part2,
    }
}
