
Console.WriteLine("Snow is falling, all around me (...)");

// PartOne();
// PartTwo();

#pragma warning disable

void PartOne()
{
    var lines = File.ReadAllLines("data.txt");

    Dictionary<Point, int> grid = new();
    Point start = new(0, 0);
    grid[start] = 1;

    var position = new CurrentPosition
    {
        Head = start,
        Tail = start
    };

    foreach (var line in lines)
    {
        var splitedLine = line.Split(" ");
        var direction = splitedLine[0];
        var count = int.Parse(splitedLine[1]);

        Action<CurrentPosition, Dictionary<Point, int>> move
            = direction switch
            {
                "U" => MoveUp,
                "D" => MoveDown,
                "L" => MoveLeft,
                "R" => MoveRight,
                _ => throw new InvalidDataException()
            };

        Enumerable.Range(0, count)
            .ToList()
            .ForEach(_ => move(position, grid));
    }

    File.WriteAllText("result.txt", grid.Keys.Count().ToString());
}

void PartTwo()
{
    var lines = File.ReadAllLines("data.txt");

    Point start = new(0, 0);

    int tailLength = 9;
    var tail = Enumerable
        .Repeat<Point>(start, tailLength)
        .ToArray();

    Dictionary<Point, int> grid = new();
    grid[start] = 1;


    var position = new CurrentPosition
    {
        Head = start,
        Tail = tail.First()
    };

    foreach (var line in lines)
    {
        var splitedLine = line.Split(" ");
        var direction = splitedLine[0];
        var count = int.Parse(splitedLine[1]);

        Action<CurrentPosition, Dictionary<Point, int>> move
            = direction switch
            {
                "U" => MoveUp,
                "D" => MoveDown,
                "L" => MoveLeft,
                "R" => MoveRight,
                _ => throw new InvalidDataException()
            };

        Enumerable.Range(1, count)
            .ToList()
            .ForEach(_ =>
            {
                move(position, null);
                tail[0] = position.Tail;
                MoveTail(tail);
                IncreaseGrid(grid, tail.Last());
            });
    }

    File.WriteAllText("result.txt", grid.Keys.Count().ToString());
}

void MoveUp(CurrentPosition position, Dictionary<Point, int>? grid = null)
{
    position.Head = new(position.Head.X, position.Head.Y + 1);

    CorrectAll(position);

    if (grid is not null)
        IncreaseGrid(grid, position.Tail);
}

void MoveDown(CurrentPosition position, Dictionary<Point, int>? grid = null)
{
    position.Head = new(position.Head.X, position.Head.Y - 1);

    CorrectAll(position);

    if (grid is not null)
        IncreaseGrid(grid, position.Tail);
}

void MoveLeft(CurrentPosition position, Dictionary<Point, int>? grid = null)
{
    position.Head = new(position.Head.X - 1, position.Head.Y);

    CorrectAll(position);

    if (grid is not null)
        IncreaseGrid(grid, position.Tail);
}

void MoveRight(CurrentPosition position, Dictionary<Point, int>? grid = null)
{
    position.Head = new(position.Head.X + 1, position.Head.Y);

    CorrectAll(position);

    if (grid is not null)
        IncreaseGrid(grid, position.Tail);
}

void IncreaseGrid(Dictionary<Point, int> grid, Point point)
{
    if (!grid.ContainsKey(point))
    {
        grid[point] = 1;
        return;
    }

    grid[point]++;
}

void MoveTail(Point[] tail)
{
    for (int i = 1; i < tail.Length; i++)
    {
        var position = new CurrentPosition
        {
            Head = tail[i - 1],
            Tail = tail[i]
        };
        CorrectAll(position);
        tail[i] = position.Tail;
    }
}

void CorrectAll(CurrentPosition position)
{
    var deltaX = position.Head.X - position.Tail.X;
    var deltaY = position.Head.Y - position.Tail.Y;
    var newX = position.Tail.X;
    var newY = position.Tail.Y;
    if (Math.Abs(deltaY) > 1 && Math.Abs(deltaX) > 1)
    {
        newX += deltaX / 2;
        newY += deltaY / 2;
        position.Tail = new Point(newX, newY);
        return;
    }

    if (Math.Abs(deltaX) > 1)
    {
        newX += deltaX / 2;
        newY = position.Head.Y;
        position.Tail = new Point(newX, newY);
        return;
    }

    if (Math.Abs(deltaY) > 1)
    {
        newY += deltaY / 2;
        newX = position.Head.X;
    }

    position.Tail = new Point(newX, newY);
}

record Point(int X, int Y);
class CurrentPosition
{
    public Point Head { get; set; }
    public Point Tail { get; set; }
}