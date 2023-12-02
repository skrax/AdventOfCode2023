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

const int MaxRed = 12;
const int MaxGreen = 13;
const int MaxBlue = 14;

while (true)
{
    var line = await reader.ReadLineAsync();
    if (line is null) break;
    ++lineCount;

    var sp1 = line.Split(':', 2);
    if (sp1.Length != 2) throw new ArgumentException("Failed to extract game id and iterations");

    var idSplit = sp1[0].Split(' ');
    if (!int.TryParse(idSplit[1], out var id))
    {
        throw new ArgumentException("Failed to parse game id");
    }

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

    if (currentRed > MaxRed || currentGreen > MaxGreen || currentBlue > MaxBlue)
    {
        continue;
    }

    possibleGames += id;
}

Console.WriteLine($"Possible Games: {possibleGames}");