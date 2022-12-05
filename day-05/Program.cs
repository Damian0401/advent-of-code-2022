
Console.WriteLine("Keep moving forward!");

// PartOne();
// PartTwo();

#pragma warning disable

void PartOne()
{
    var data = File.ReadAllText("data.txt")
        .Split("\r\n\r\n");

    var stackInfo = data[0]
        .Split('\n');

    var stackAmount = stackInfo
        .Last()
        .Trim()
        .Split("   ")
        .Count();

    Stacks stacks = new Stacks(stackAmount);

    var rows = stackInfo[..^1];

    foreach (var row in rows)
    {
        var items = GetItemsFromRow(row);

        AddItemsToStacks(items, stacks);
    }

    var moves = data[1]
        .Split('\n')
        .Select(x => x.Split(' '))
        .Select(x => 
            new Move(int.Parse(x[1]), int.Parse(x[3]) - 1, int.Parse(x[5]) - 1));

    foreach (var move in moves)
        stacks.MoveItems(move);

    var result = stacks.GetItemsFromTop();

    File.WriteAllText("result.txt", result);
}

void PartTwo()
{
    var data = File.ReadAllText("data.txt")
        .Split("\r\n\r\n");

    var stackInfo = data[0]
        .Split('\n');

    var stackAmount = stackInfo
        .Last()
        .Trim()
        .Split("   ")
        .Count();

    Stacks stacks = new Stacks(stackAmount);

    var rows = stackInfo[..^1];

    foreach (var row in rows)
    {
        var items = GetItemsFromRow(row);

        AddItemsToStacks(items, stacks);
    }

    var moves = data[1]
        .Split('\n')
        .Select(x => x.Split(' '))
        .Select(x => 
            new Move(int.Parse(x[1]), int.Parse(x[3]) - 1, int.Parse(x[5]) - 1));

    foreach (var move in moves)
        stacks.MoveItemsWithoutReversing(move);

    var result = stacks.GetItemsFromTop();

    File.WriteAllText("result.txt", result);
}

List<char> GetItemsFromRow(string row)
{
    return row
        .Select((item, index) => new { item, index})
        .GroupBy(x => x.index / 4)
        .Select(x => x.Select(i => i.item).ToList())
        .Select(x => x[1])
        .ToList();
}

void AddItemsToStacks(List<char> items, Stacks stacks)
{
    for (int i = 0; i < items.Count(); i++)
    {
        if (items[i] != ' ')
        {
            stacks.AddItemToStack(items[i], i);
        }
    }    
}

record Move(int Amount, int From, int To);

class Stacks
{
    private readonly List<char>[] _stacks;

    public Stacks(int stackAmount)
    {
        _stacks = new List<char>[stackAmount];
        for (int i = 0; i < stackAmount; i++)
        {
            _stacks[i] = new List<char>();
        }
    }

    public void AddItemToStack(char item, int stackNumber)
    {
        _stacks[stackNumber].Insert(0, item);
    }

    public void PrintStacks()
    {
        foreach (var stack in _stacks)
        {
            foreach (var item in stack)
            {
                Console.Write(item);
            }
            Console.WriteLine();
        }
    }

    public void MoveItems(Move move)
    {
        var itemsToMove = _stacks[move.From]
            .TakeLast(move.Amount)
            .ToList();

        itemsToMove.Reverse();

        var firstIndexToRemove = _stacks[move.From].Count() - move.Amount;
        _stacks[move.From]
            .RemoveRange(firstIndexToRemove, move.Amount);

        _stacks[move.To].AddRange(itemsToMove);
    }

    public void MoveItemsWithoutReversing(Move move)
    {
        var itemsToMove = _stacks[move.From]
            .TakeLast(move.Amount)
            .ToList();

        var firstIndexToRemove = _stacks[move.From].Count() - move.Amount;
        _stacks[move.From]
            .RemoveRange(firstIndexToRemove, move.Amount);

        _stacks[move.To].AddRange(itemsToMove);
    }

    public string GetItemsFromTop()
    {
        var itemsFromTop = _stacks
            .Select(x => x.Last())
            .ToArray();

        return new string(itemsFromTop);
    }
}