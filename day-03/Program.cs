
Console.WriteLine("Hello there!");

// PartOne();
// PartTwo();

#pragma warning disable

void PartOne()
{
    var data = File.ReadAllLines("data.txt");

    var rucksacks = data.Select(x => new Rucksack(x[..(x.Length/2)], x[(x.Length/2)..]));

    var items = rucksacks.Select(x => 
    {
        foreach (var item in x.FirstCompartment)
        {
            if (x.SecondCompartment.Contains(item))
                return item;
        }
        throw new InvalidDataException();
    });

    var result = items
        .Select(x => GetPriority(x))
        .Sum();

    File.WriteAllText("result.txt", result.ToString());
}

void PartTwo()
{
    var data = File.ReadAllLines("data.txt");

    var rucksacks = data
        .Select((item, index) => new { item, index})
        .GroupBy(x => x.index / 3)
        .Select(x => x.Select(i => i.item).ToList());

    var items = rucksacks.Select(x =>
    {
        foreach (var item in x[0])
        {
            if (x[1].Contains(item) && x[2].Contains(item))
                return item;
        }
        throw new InvalidDataException();
    });

    var result = items
        .Select(x => GetPriority(x))
        .Sum();

    File.WriteAllText("result.txt", result.ToString());
}

int GetPriority(char item)
{
    if (item >= 'a' && item <= 'z')
        return item - 'a' + 1;

    return item - 'A' + 27;
}

record Rucksack(string FirstCompartment, string SecondCompartment);