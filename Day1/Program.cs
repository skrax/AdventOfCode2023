using StreamReader = System.IO.StreamReader;

var cmd = Environment.GetCommandLineArgs().Skip(1);
var filePath = cmd.FirstOrDefault();

if (filePath is null)
{
    throw new ArgumentException("Provide a file to process");
}

using var reader = new StreamReader(filePath);

var result = 0;
var lineCount = 0;

var options = new[] {"one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};

while (true)
{
    var line = await reader.ReadLineAsync();
    if (line is null) break;
    ++lineCount;

    string? first = null;
    string? second = null;

    void SetNumber(string number)
    {
        if (first is null)
        {
            first = number;
        }
        else
        {
            second = number;
        }
    }


    for (var index = 0; index < line.Length; index++)
    {
        var letter = line[index];
        var possibleWordLength = line.Length - index;

        string? GetMaybe(int requiredLength)
        {
            return possibleWordLength < requiredLength ? null : line[index..(index + requiredLength)];
        }

        bool IsWord(string word)
        {
            return GetMaybe(word.Length) == word;
        }

        if (int.TryParse(letter.ToString(), out _))
        {
            SetNumber(letter.ToString());
        }
        else
        {
            for (var i = 0; i < options.Length; i++)
            {
                var option = options[i];
                if (IsWord(option))
                {
                    SetNumber((i+1).ToString());
                    index += option.Length - 2;
                    break;
                }
            }
        }
    }

    if (first is not null && second is not null)
    {
        var number = int.Parse($"{first}{second}");
        result += number;
    }
    else if (first is not null)
    {
        var number = int.Parse($"{first}{first}");
        result += number;
    }
    else
    {
        throw new ArgumentException($"line {lineCount} has no digits!");
    }
}

Console.WriteLine($"Result is: {result}");