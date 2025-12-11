static class Day04
{
    internal static void Run(TextReader reader)
    {
        HashSet<(int Row, int Col)> rolls = reader.GetRollsPositions();
        int resultPart1 = rolls.GetNumRemoved(1);
        Console.WriteLine($"Answer to Part 1 = {resultPart1}");
        int resultPart2 = rolls.GetNumRemoved();
        Console.WriteLine($"Answer to Part 2 = {resultPart2}");
    }

    private static int GetNumRemoved(this HashSet<(int Row, int Col)> coords, int remainingAttempts = int.MaxValue) =>
        remainingAttempts == 0 ? 0 :
            coords.FindRemovableRolls() switch
            {
                { Count: 0 } => 0,
                {} removables => removables.Count + coords.Except(removables).ToHashSet().GetNumRemoved(remainingAttempts - 1),
            };

    private static HashSet<(int Row, int Col)> FindRemovableRolls(this HashSet<(int Row, int Col)> coords) => coords
        .Where(coords.Accessible)
        .ToHashSet();

    private static bool Accessible(this HashSet<(int Row, int Col)> coords, (int Row, int Col) pair) =>
    (
        from r in new int[] { -1, 0, 1 }
        from c in new int[] { -1, 0, 1 }
        let neighbour = (pair.Row + r, pair.Col + c)
        where (r, c) is not (0, 0) && coords.Contains(neighbour)
        select neighbour
    )
        .Count() < 4;

    private static HashSet<(int Row, int Col)> GetRollsPositions(this TextReader reader) => reader.ReadLines()
        .SelectMany((r, row) => r.Select((item, col) => (row, col, isRoll: item == '@')))
        .Where(t => t.isRoll)
        .Select(t => (t.row, t.col))
        .ToHashSet();
}
