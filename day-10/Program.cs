
Console.WriteLine("Feliz Navidad!");

// PartOne();
PartTwo();

#pragma warning disable

void PartOne()
{
    var lines = File.ReadAllLines("data.txt");

    int result = 0;
    int registerValue = 1;
    int currentCycle = 1;
    int nextSample = 20;
    int offset = 40;

    foreach (var line in lines)
    {
        var splitedLine = line.Split(" ");
        var command = splitedLine[0];

        TryTakeSample(currentCycle, offset,
            registerValue, ref nextSample, ref result);
        currentCycle++;

        if (command.Equals("addx"))
        {
            TryTakeSample(currentCycle, offset,
                registerValue, ref nextSample, ref result);
            currentCycle++;
            registerValue += int.Parse(splitedLine[1]);
        }

    }

    File.WriteAllText("result.txt", result.ToString());
}

void PartTwo()
{
    var lines = File.ReadAllLines("data.txt");

    var result = new char[6][]
    {
        Enumerable.Repeat('@', 40).ToArray(),
        Enumerable.Repeat('@', 40).ToArray(),
        Enumerable.Repeat('@', 40).ToArray(),
        Enumerable.Repeat('@', 40).ToArray(),
        Enumerable.Repeat('@', 40).ToArray(),
        Enumerable.Repeat('@', 40).ToArray(),
    };

    int registerValue = 1;
    int currentCycle = 0;
    int currentRow = 0;
    int rowWidth = 0;
    int rowLength = 40;

    foreach (var line in lines)
    {
        var splitedLine = line.Split(" ");
        var command = splitedLine[0];

        DrawCurrentPixel(result[currentRow], currentCycle, registerValue, rowLength);
        UpdateCycle(ref currentCycle, ref currentRow, ref rowWidth, rowLength);

        if (command.Equals("addx"))
        {
            DrawCurrentPixel(result[currentRow], currentCycle, registerValue, rowLength);
            UpdateCycle(ref currentCycle, ref currentRow, ref rowWidth, rowLength);

            registerValue += int.Parse(splitedLine[1]);
        }
    }

    File.WriteAllLines("result.txt", result.Select(x => new string(x)));
}

void TryTakeSample(int currentCycle, int offset, 
    int registerValue, ref int nextSample, ref int result)
{
    if (!currentCycle.Equals(nextSample))
        return;

    result += (currentCycle * registerValue);
    nextSample += offset;
}

void DrawCurrentPixel(char[] row, int currentCycle, int spritePosition, int rowLength)
{
    if (IsInRange(currentCycle % rowLength, spritePosition))
    {
        row[currentCycle % rowLength] = '#';
        return;
    }

    row[currentCycle % rowLength] = '.';
}

bool IsInRange(int currentCycle, int spritePosition)
{
    return Math.Abs(spritePosition - currentCycle) < 2;
}

void UpdateCycle(ref int currentCycle, ref int currentRow, 
    ref int rowWidth, int rowLength)
{
    currentCycle++;
    rowWidth++;

    if (rowWidth >= rowLength)
    {
        rowWidth = 0;
        currentRow++;
    }
}