
Console.WriteLine("Have a nice day!");

// PartOne();
// PartTwo();


void PartOne()
{
    var data = File.ReadAllText("data.txt");

    var groupedData = data.Split("\r\n\r\n")
        .Select(x => x.Split("\n"));

    var parsetData = groupedData
        .Select(x => 
            x.Select(y => int.Parse(y)));

    var result = parsetData
        .Select(x => x.Sum())
        .Max();

    File.WriteAllText("result.txt", result.ToString());
}

void PartTwo()
{
    var data = File.ReadAllText("data.txt");

    var groupedData = data.Split("\r\n\r\n")
        .Select(x => x.Split("\n"));

    var parsetData = groupedData
        .Select(x => 
            x.Select(y => int.Parse(y)));

    var result = parsetData
        .Select(x => x.Sum())
        .OrderByDescending(x => x)
        .Take(3)
        .Sum();

    File.WriteAllText("result.txt", result.ToString());
}

// PartOne
// var result = data.Split("\r\n\r\n")
//     .Select(x => x.Split("\n")
//         .Select(y => int.Parse(y))
//             .Sum()).Max();

// PartTwo
// var result = data.Split("\r\n\r\n")
//     .Select(x => x.Split("\n")
//         .Select(y => int.Parse(y)).Sum())
//     .OrderByDescending(x => x)
//     .Take(3).Sum();