
Console.WriteLine("Don't worry, be happy!");

// PartOne();
// PartTwo();

#pragma warning disable

void PartOne()
{
    var data = File.ReadAllText("data.txt");

    int codeLength = 4;

    for (int i = codeLength; i < data.Length; i++)
    {
        var uniqueElements = data[(i - codeLength)..i]
            .Distinct();

        if (uniqueElements.Count() == codeLength)
        {
            File.WriteAllText("result.txt", i.ToString());
            break;
        }
    }
}

void PartTwo()
{
    var data = File.ReadAllText("data.txt");

    int codeLength = 14;

    for (int i = codeLength; i < data.Length; i++)
    {
        var uniqueElements = data[(i - codeLength)..i]
            .Distinct();

        if (uniqueElements.Count() == codeLength)
        {
            File.WriteAllText("result.txt", i.ToString());
            break;
        }
    }
}
