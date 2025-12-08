static class Day04
{
    internal static void Run(TextReader reader)
    {
        bool[][] grid = reader.GetGrid();
        int resultPart1 = grid.FindForkLiftPositions().Count();
        Console.WriteLine($"Answer to Part 1 = {resultPart1}");
    }

    private static IEnumerable<(int Row, int Col)> GetAllCoordinatesOfDimensions(this (int row, int col) start,
                                                                          (int row, int col) size) => Enumerable
        .Range(start.row, size.row)
        .SelectMany(r => Enumerable.Range(start.col, size.col), (row, col) => (row, col));

    private static IEnumerable<(int Row, int Col)> GetAllNeighboursWithin(this (int row, int col) pos,
                                                                          (int row, int col) boundary) => (pos.row - 1, pos.col - 1)
        .GetAllCoordinatesOfDimensions((3, 3))
        .Where(p =>
            p.Row >= 0 && p.Row < boundary.row          // Row boundary
            && p.Col >= 0 && p.Col < boundary.col       // Col boundary
            && (p.Row != pos.row || p.Col != pos.col)); // Exclude itself

    private static (int Row, int Col) Dimensions(this bool[][] grid) => (grid.Length, grid[0].Length);

    private static IEnumerable<(int Row, int Col)> FindForkLiftPositions(this bool[][] grid) => (0, 0)
        .GetAllCoordinatesOfDimensions(grid.Dimensions())
        .Where(pos => grid[pos.Row][pos.Col])
        .Where(pos => pos.GetAllNeighboursWithin(grid.Dimensions()).Count(p => grid[p.Row][p.Col]) < 4);

    private static bool[][] GetGrid(this TextReader reader) => reader.ReadLines()
        .Select(r => r.Select(c => c == '@').ToArray())
        .ToArray();
}
