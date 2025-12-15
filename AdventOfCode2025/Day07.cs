static class Day07
{
    internal static void Run(TextReader reader)
    {
        int beam = reader.GetInitialBeam();
        List<HashSet<int>> splitters = reader.GetSplitters().ToList();
        int resultPart1 = splitters.FindSplits(beam).Select(s => s.Count).Sum();
        Console.WriteLine($"Answer to Part 1 = {resultPart1}");
    }

    private static HashSet<int> Apply(this HashSet<int> beams, HashSet<int> splitters) => beams
        .SelectMany(splitters.Split)
        .ToHashSet();

    private static IEnumerable<int> Split(this HashSet<int> splitters, int beam) => splitters.Contains(beam)
        ? [beam - 1, beam + 1] : [beam];

    private static IEnumerable<HashSet<int>> FindSplits(this IEnumerable<HashSet<int>> splittersRows, int initialBeam)
    {
        HashSet<int> beams = new HashSet<int> { initialBeam };
        foreach (HashSet<int> splitters in splittersRows)
        {
            yield return splitters.Intersect(beams).ToHashSet();
            beams = beams.Apply(splitters);
        }
    }

    private static int GetInitialBeam(this TextReader reader) => reader
        .ReadLine()!
        .IndexOf('S');

    private static IEnumerable<HashSet<int>> GetSplitters(this TextReader reader) => reader
        .ReadLines()
        .Select(line => line
            .Select((c, idx) => (c, idx))
            .Where(p => p.c == '^')
            .Select(p => p.idx)
            .ToHashSet())
        .Where(s => s.Count > 0);
        
}
