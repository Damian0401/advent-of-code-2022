
Console.WriteLine("Let it snow! let it snow! let it snow!");

// PartOne();
PartTwo();

#pragma warning disable

void PartOne()
{
    var data = File.ReadAllLines("data.txt");

    int height = data.Length;
    int width = data.First().Length;

    int[][] grid = new int[height][];

    for (int i = 0; i < data.Length; i++)
    {
        var trees = data[i]
            .Select(x => int.Parse(x.ToString()))
            .ToArray();

        grid[i] = trees;
    }

    int result = 0;

    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            var tree = grid[y][x];

            var left = grid[y][0..x];
            if (left.All(x => x < tree))
            {
                result++;
                continue;
            }

            var right = grid[y][(x+1)..width];
            if (right.All(x => x < tree))
            {
                result++;
                continue;
            }

            var up = grid[0..y].Select(row => row[x]);
            if (up.All(x => x < tree))
            {
                result++;
                continue;
            }

            var down = grid[(y+1)..height].Select(row => row[x]);
            if (down.All(x => x < tree))
                result++;
        }
    }

    File.WriteAllText("result.txt", result.ToString());
}

void PartTwo()
{
    var data = File.ReadAllLines("data.txt");

    int height = data.Length;
    int width = data.First().Length;

    int[][] grid = new int[height][];

    for (int i = 0; i < data.Length; i++)
    {
        var trees = data[i]
            .Select(x => int.Parse(x.ToString()))
            .ToArray();

        grid[i] = trees;
    }

    int bestScore = 0;

    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            var up = CountUp(grid, x, y);
            var down = CountDown(grid, x, y);
            var left = CountLeft(grid, x, y);
            var right = CountRight(grid, x, y);

            var score = up * down * left * right;
            if (score > bestScore)
                bestScore = score;
        }
    }

    File.WriteAllText("result.txt", bestScore.ToString());
}

int CountUp(int[][] grid, int x, int y)
{
    var tree = grid[y][x];
    int count = 0;

    for (int i = y - 1; i >= 0 ; i--)
    {
        count++;

        if (grid[i][x] >= tree)
            break;
    }

    return count;
}

int CountDown(int[][] grid, int x, int y)
{
    var tree = grid[y][x];
    int count = 0;

    for (int i = y + 1; i < grid.Length; i++)
    {
        count++;

        if (grid[i][x] >= tree)
            break;
    }

    return count;
}

int CountLeft(int[][] grid, int x, int y)
{
    var tree = grid[y][x];
    int count = 0;

    for (int i = x - 1; i >= 0 ; i--)
    {
        count++;

        if (grid[y][i] >= tree)
            break;
    }

    return count;
}

int CountRight(int[][] grid, int x, int y)
{
    var tree = grid[y][x];
    int count = 0;

    for (int i = x + 1; i < grid.Length; i++)
    {
        count++;

        if (grid[y][i] >= tree)
            break;
    }

    return count;
}