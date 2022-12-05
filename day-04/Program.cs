
Console.WriteLine("To be, or not to be...");

// PartOne();
// PartTwo();

#pragma warning disable

void PartOne()
{
    var data = File.ReadAllLines("data.txt");

    int count = 0;

    foreach (var line in data)
    {
        var joinedParts = line.Split(',');

        var firstPart = joinedParts[0]
            .Split('-');
        var firstElf = 
            new Elf(int.Parse(firstPart.First()), int.Parse(firstPart.Last()));

        var secondPart = joinedParts[1]
            .Split('-');
        var secondElf = 
            new Elf(int.Parse(secondPart.First()), int.Parse(secondPart.Last()));

        var pair = new Pair(firstElf, secondElf);

        if (FullyContains(pair))
            count++;
    }

    File.WriteAllText("result.txt", count.ToString());
}

void PartTwo()
{
    var data = File.ReadAllLines("data.txt");

    int count = 0;

    foreach (var line in data)
    {
        var joinedParts = line.Split(',');

        var firstPart = joinedParts[0]
            .Split('-');
        var firstElf = 
            new Elf(int.Parse(firstPart.First()), int.Parse(firstPart.Last()));

        var secondPart = joinedParts[1]
            .Split('-');
        var secondElf = 
            new Elf(int.Parse(secondPart.First()), int.Parse(secondPart.Last()));

        var pair = new Pair(firstElf, secondElf);

        if (Overlap(pair))
            count++;
    }

    File.WriteAllText("result.txt", count.ToString());
}

bool Overlap(Pair pair)
{
    var firstElf = pair.FirstElf;
    var secondElf = pair.SecondElf;

    if (firstElf.First < secondElf.First && firstElf.Last < secondElf.First)
        return false;

    if (firstElf.First > secondElf.Last && firstElf.Last > secondElf.Last)
        return false;

    return true;   
}

bool FullyContains(Pair pair)
{
    var firstElf = pair.FirstElf;
    var secondElf = pair.SecondElf;

    if (firstElf.First <= secondElf.First && firstElf.Last >= secondElf.Last)
    {
        return true;
    }

    if (secondElf.First <= firstElf.First && secondElf.Last >= firstElf.Last)
    {
        return true;
    }

    return false;
}

record Elf(int First, int Last);
record Pair(Elf FirstElf, Elf SecondElf);

// var pairs = data
//     .Select(x => x
//         .Split(',')
//         .Select(y => y
//             .Split('-'))
//         .Select(z => 
//             new Elf(int.Parse(z.First()), int.Parse(z.Last()))))
//     .Select(k => new Pair(k.First(), k.Last()));