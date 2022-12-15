
Console.WriteLine("I won't gonna watch 'Planet of the Apes' for a while...");

// PartOne();
PartTwo();

#pragma warning disable

void PartOne()
{
    var monkeysData = File.ReadAllText("data.txt")
        .Split("\r\n\r\n");

    List<Monkey> monkeys = new();

    foreach (var data in monkeysData)
    {
        var monkey = Monkey.Parse(data);

        monkeys.Add(monkey);
    }

    Enumerable.Range(0, 20)
        .ToList()
        .ForEach(_ => ExecuteRound(monkeys, divider: 3));

    var result = monkeys
        .OrderByDescending(x => x.InspectedItems)
        .Take(2)
        .Select(x => x.InspectedItems);

    var monkeyBusiness = result.First() * result.Last();

    File.WriteAllText("result.txt", monkeyBusiness.ToString());
}

void PartTwo()
{
    var monkeysData = File.ReadAllText("data.txt")
        .Split("\r\n\r\n");

    List<Monkey> monkeys = new();

    foreach (var data in monkeysData)
    {
        var monkey = Monkey.Parse(data);

        monkeys.Add(monkey);
    }

    var dividers = monkeys.Select(x => x.TestDivider)
        .ToList();

    var nww = FindNWW(dividers);

    Enumerable.Range(0, 10000)
        .ToList()
        .ForEach(_ => ExecuteRound(monkeys, modulo: nww));

    var result = monkeys
        .OrderByDescending(x => x.InspectedItems)
        .Take(2)
        .Select(x => x.InspectedItems);

    var monkeyBusiness = (long)result.First() * (long)result.Last();

    File.WriteAllText("result.txt", monkeyBusiness.ToString());
}

void ExecuteRound(List<Monkey> monkeys, long? divider = null,
    long? modulo = null)
{
    foreach (var monkey in monkeys)
    {
        ExamineAllItems(monkey, monkeys, divider, modulo);
    }
}

void ExamineAllItems(Monkey monkey, List<Monkey> monkeys, long? divider = null,
    long? modulo = null)
{
    while (monkey.Items.Count() > 0)
    {
        ExamineItem(monkey, monkeys, divider, modulo);
    }
}

void ExamineItem(Monkey monkey, List<Monkey> monkeys, long? divider = null,
    long? modulo = null)
{
    var item = monkey.Items.First();
    monkey.Items.RemoveAt(0);

    var result = monkey
        .Operation
        .Invoke(item);

    if (divider is not null)
        result /= divider.Value;

    if (modulo is not null)
        result %= modulo.Value;

    var isSuccess = monkey.Test(result);
    monkey.InspectedItems++;

    if (isSuccess)
    {
        monkeys[monkey.IfTrueIndex]
            .Items
            .Add(result);

        return;
    }

    monkeys[monkey.IfFalseIndex]
        .Items
        .Add(result);
}

long FindNWW(List<int> numbers)
{
    long result = numbers.Max();

    while (numbers.Any(x => result % x != 0))
    {
        result++;
    }

    return result;
}

class Monkey
{
    public int TestDivider { get; set; }
    public List<long> Items { get; set; }
    public Operation Operation { get; set; }
    public int IfTrueIndex { get; set; }
    public int IfFalseIndex { get; set; }
    public int InspectedItems { get; set; } = 0;

    public bool Test(long value)
    {
        return (value % TestDivider).Equals(0);
    }

    public static Monkey Parse(string data)
    {
        var lines = data.Split("\n");

        var items = lines[1]
            .Split(": ")
            .Last()
            .Split(", ")
            .Select(x => long.Parse(x));

        var operationElements = lines[2]
            .Split("= ")
            .Last()
            .Split(" ");

        long? firstElement = null;
        if (long.TryParse(operationElements[0], out var first))
        {
            firstElement = first;
        }

        Func<long, long, long> operation = operationElements[1] == "*"
            ? (x, y) => x * y
            : (x, y) => x + y;

        long? secondElement = null;
        if (long.TryParse(operationElements[2], out var second))
        {
            secondElement = second;
        }

        var testDivider = lines[3]
            .Split(' ')
            .Last();

        var ifTrueIndex = lines[4]
            .Split(' ')
            .Last();

        var ifFalseIndex = lines[5]
            .Split(' ')
            .Last();

        Monkey monkey = new Monkey
        {
            Items = items.ToList(),
            Operation = new Operation(operation, firstElement, secondElement),
            TestDivider = int.Parse(testDivider),
            IfTrueIndex = int.Parse(ifTrueIndex),
            IfFalseIndex = int.Parse(ifFalseIndex)
        };

        return monkey;
    }

}

class Operation
{
    private Func<long, long, long> _operation;
    private long? _leftElement;
    private long? _rightElement;

    public Operation(Func<long, long, long> operation,
        long? leftElement = null, long? rightElement = null)
    {
        _operation = operation;
        _leftElement = leftElement;
        _rightElement = rightElement;
    }

    public long Invoke(long old)
    {
        return _operation(_leftElement ?? old, _rightElement ?? old);
    }
}