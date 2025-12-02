Action<TextReader>[] challenges =
[
    Day01.Run,
];

TextReader GetInput(int dayNumber) => Directory.GetFiles(Directory.GetCurrentDirectory(), $"Day{dayNumber:D2}.txt", SearchOption.AllDirectories) switch
{
    [string filePath, ..] => new StreamReader(filePath),
    _ => Console.In,
};

int dayNumber = 0;
do
{
    Console.WriteLine("Please enter day number of challenge: ");
    string input = Console.ReadLine() ?? string.Empty;
    if (!int.TryParse(input, out dayNumber))
    {
        Console.WriteLine("Invalid input, try again\n");
        continue;
    }
    if (dayNumber <= 0 || dayNumber > challenges.Length)
    {
        Console.WriteLine("Input number out of bounds, try again\n");
        continue;
    }
    break;
} while (true);

challenges[dayNumber - 1](GetInput(dayNumber));
