
Console.WriteLine("Last christmas I gave you my heart (...)");

// PartOne();
PartTwo();

#pragma warning disable

void PartOne()
{
    var data = File.ReadAllLines("data.txt");

    var root = ParseCommands(data);

    var directories = FindDirectoriesWithMaxSize(root, 100000);

    var result = directories
        .Select(x => CalculateTotalSize(x))
        .Sum();

    File.WriteAllText("result.txt", result.ToString());
}

void PartTwo()
{
    var data = File.ReadAllLines("data.txt");

    var root = ParseCommands(data);

    List<int> directorySizes = new();

    int maxSize = 70000000;
    int requiredSize = 30000000;
    int currentSize = CalculateTotalSize(root, directorySizes);
    int availableSize = maxSize - currentSize;
    int neededSize = requiredSize - availableSize;

    int result = directorySizes
        .OrderBy(x => x)
        .FirstOrDefault(x => x > neededSize);

    File.WriteAllText("result.txt", result.ToString());
}

Directory ParseCommands(string[] commands)
{
    var root = new Directory(null, "/");
    var currentDirectory = root;

    foreach (var command in commands)
    {
        var splitedLine = command.Split(' ');

        if (splitedLine[1].Equals("ls"))
            continue;

        if (splitedLine[1].Equals("cd"))
        {
            var directoryName = splitedLine[2];

            if (directoryName.Equals("/"))
            {
                currentDirectory = root;
                continue;
            }
            
            if (directoryName.Equals(".."))
            {
                currentDirectory = currentDirectory.Parent;
                continue;
            }
            
            var directoryToMove = currentDirectory
                .Children
                .FirstOrDefault(d => 
                    d.Name.Equals(directoryName));

            if (directoryToMove is null)
                throw new InvalidDataException();

            currentDirectory = directoryToMove;
            continue;
        }            

        if (splitedLine[0].Equals("dir"))
        {
            currentDirectory
                .Children
                .Add(new Directory(currentDirectory, splitedLine[1]));

            continue;
        }

        int fileSize = int.Parse(splitedLine[0]);

        currentDirectory.Files
            .Add(new SimpleFile(splitedLine[1], fileSize));
    }

    return root;
}

List<Directory> FindDirectoriesWithMaxSize(Directory root, int maxSize)
{
    List<Directory> result = new();

    ExamineNextLevel(result, root, maxSize);

    return result;
}

int ExamineNextLevel(List<Directory> directories, Directory currentDirectory, 
    int maxSize)
{
    int currentSize = currentDirectory
        .Files.Sum(x => x.Size);
    
    foreach (Directory directory in currentDirectory.Children)
    {
        currentSize += ExamineNextLevel(directories, directory, maxSize);
    }

    if (currentSize < maxSize)
        directories.Add(currentDirectory);

    return currentSize;
}

int CalculateTotalSize(Directory directory, List<int> sizes = null)
{
    int totalSize = directory
        .Files
        .Sum(x => x.Size);

    foreach (Directory child in directory.Children)
    {
        totalSize += CalculateTotalSize(child, sizes);
    }

    if (sizes is not null)
        sizes.Add(totalSize);

    return totalSize;
}

record SimpleFile(string FileName, int Size);
class Directory 
{
    public Directory(Directory? parent, string name)
    {
        Parent = parent;
        Name = name;
    }

    public string Name { get; set; }
    public Directory? Parent { get; set; }
    public List<Directory> Children { get; set; } = new();
    public List<SimpleFile> Files { get; set; } = new();
}