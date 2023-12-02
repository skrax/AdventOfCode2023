using StreamReader = System.IO.StreamReader;

var cmd = Environment.GetCommandLineArgs().Skip(1);
var filePath = cmd.FirstOrDefault();

if (filePath is null)
{
    throw new ArgumentException("Provide a file to process");
}

using var reader = new StreamReader(filePath);

var lineCount = 0;
var possibleGames = 0;

while (true)
{
    var line = await reader.ReadLineAsync();
    if (line is null) break;
    ++lineCount;

    var sp1 = line.Split(':', 2);
    if (sp1.Length != 2) throw new ArgumentException("Failed to extract game id and iterations");

    var iterations = sp1[1].Split(';');

    var currentRed = 0;
    var currentGreen = 0;
    var currentBlue = 0;

    foreach (var iteration in iterations)
    {
        var picks = iteration.Split(',');
        foreach (var pick in picks)
        {
            var split = pick.Trim().Split(' ', 2);
            if (split.Length != 2) throw new ArgumentException("Failed to extract picked color");

            if (!int.TryParse(split.First(), out var count))
            {
                throw new ArgumentException("Failed to parse color count");
            }

            var color = split.Last();

            switch (color)
            {
                case "red":
                {
                    currentRed = int.Max(currentRed, count);
                    break;
                }
                case "green":
                {
                    currentGreen = int.Max(currentGreen, count);
                    break;
                }
                case "blue":
                {
                    currentBlue = int.Max(currentBlue, count);
                    break;
                }
                default:
                {
                    throw new ArgumentException("Unknown color");
                }
            }
        }
    }

    var power = currentRed * currentBlue * currentGreen;

    possibleGames += power;
}

Console.WriteLine($"Possible Games: {possibleGames}");