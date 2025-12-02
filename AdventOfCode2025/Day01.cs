static class Day01
{
    internal static void Run(TextReader reader)
    {
        int result = reader.LoadMoves().StartPassAt(50, 0).Count;
        Console.Write(result);
    }

    private static (int Position, int Count) StartPassAt(this IEnumerable<int> rotations, int initialValue, int passingPoint) =>
        passingPoint is < 0 or >= 100 ? throw new ArgumentOutOfRangeException() :
        rotations.Aggregate((initialValue, 0),
            ((int, int) state, int rotation) => state.Move(rotation, passingPoint));

    private static (int Position, int Count) Move(this (int position, int count) state, int rotation, int passingPoint)
    {
        int newPosition = state.position.Apply(rotation);
        return (newPosition, state.count + (newPosition.Passes(passingPoint) ? 1 : 0));
    }

    private static int Apply(this int position, int rotation) => (position + rotation + 100) % 100;

    private static bool Passes(this int position, int passingPoint) => passingPoint == position;

    private static IEnumerable<int> LoadMoves(this TextReader reader) => reader.ReadLines().Select(ParseMove);

    private static int ParseMove(this string move) => move switch
    {
        ['L', .. string num] => -int.Parse(num),
        ['R', .. string num] => int.Parse(num),
        _ => throw new NotSupportedException(),
    };
}
