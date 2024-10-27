using Advent.Common;

namespace A2022.Problem07;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var root = LoadFile(filename);

        var flattenDirs = Flatten(root).OfType<FileSystemDir>();

        const int maximumSize = 100000;

        var result = flattenDirs
            .Where(a => a != root)
            .Where(a => a.CalculatedSize < maximumSize)
            .Sum(a => a.CalculatedSize);

        return result;
    }

    public long RunB(string filename)
    {
        var root = LoadFile(filename);

        var flattenDirs = Flatten(root).OfType<FileSystemDir>();

        const int totalSpace = 70_000_000;
        const int freeSpaceRequired = 30_000_000;

        var currentFreeSpace = totalSpace - root.CalculatedSize;
        var needToFree = freeSpaceRequired - currentFreeSpace;

        var ordered = flattenDirs
            .Where(a => a.CalculatedSize >= needToFree)
            .OrderBy(a => a.CalculatedSize)
            .First();

        return ordered.CalculatedSize;
    }

    private static FileSystemDir LoadFile(string filename)
    {
        var lines = File.ReadAllLines(filename);
        var commands = SplitToCommands(lines);
        var root = CreateFileSystem(commands);

        CalculateFolderSizes(root);

        return root;
    }

    static IEnumerable<FileSystemItem> Flatten(FileSystemDir parent)
    {
        foreach (var child in parent.Children)
            yield return child;

        foreach (var child in parent.Children.OfType<FileSystemDir>())
            foreach (var item in Flatten(child))
                yield return item;
    }

    static void CalculateFolderSizes(FileSystemDir parent)
    {
        foreach (var child in parent.Children.OfType<FileSystemDir>())
            CalculateFolderSizes(child);

        parent.CalculatedSize = parent.Children
            .Sum(a => a switch
            {
                FileSystemDir fsd => fsd.CalculatedSize,
                FileSystemFile fsf => fsf.Size,
            });
    }

    static FileSystemDir CreateFileSystem(IEnumerable<Command> commands)
    {
        var root = new FileSystemDir(@"/", null);
        var currentDir = root;

        foreach (var command in commands)
        {
            if (command is CommandCd cd)
            {
                if (cd.Dir == "/")
                {
                    currentDir = root;
                }
                else if (cd.Dir == "..")
                {
                    currentDir = currentDir.Parent ?? throw new("Parent is null");
                }
                else
                {
                    var existing = currentDir.Children
                        .OfType<FileSystemDir>()
                        .FirstOrDefault(a => a.Name == cd.Dir);

                    if (existing is null)
                    {
                        existing = new FileSystemDir(cd.Dir, currentDir);
                        currentDir.Children.Add(existing);
                    }

                    currentDir = existing;
                }
            }
            else if (command is CommandLs ls)
            {
                foreach (var item in ls.Output)
                {
                    var existing = currentDir.Children
                        .FirstOrDefault(a => a.Name == item.Name);

                    if (existing is null)
                    {
                        FileSystemItem fs = item switch
                        {
                            CommandLsDir cld => new FileSystemDir(cld.Name, currentDir),
                            CommandLsFile clf => new FileSystemFile(clf.Name, clf.Size),
                        };

                        currentDir.Children.Add(fs);
                    }
                }
            }
        }

        return root;
    }

    static IEnumerable<Command> SplitToCommands(IEnumerable<string> lines)
    {
        const string prefix = "$ ";
        const string cdPrefix = "cd ";

        foreach (var chunk in lines.Split(a => a.StartsWith(prefix)))
        {
            if (chunk.ToArray() is [var input, .. var output])
            {
                var command = input[prefix.Length..];

                if (command.StartsWith(cdPrefix))
                    yield return new CommandCd(command[cdPrefix.Length..]);
                else if (command == "ls")
                    yield return new CommandLs(output.Select(Parse).ToArray());
            }
        }
    }

    static CommandLsItem Parse(string line)
    {
        const string dirPrefix = "dir ";

        if (line.StartsWith(dirPrefix))
            return new CommandLsDir(line[dirPrefix.Length..]);

        var n = line.IndexOf(' ');

        return new CommandLsFile(line[(n + 1)..], long.Parse(line[0..n]));
    }
}

abstract record Command;
record CommandCd(string Dir) : Command;
record CommandLs(CommandLsItem[] Output) : Command;
abstract record CommandLsItem(string Name);
record CommandLsFile(string Name, long Size) : CommandLsItem(Name);
record CommandLsDir(string Name) : CommandLsItem(Name);

class FileSystemItem(string name)
{
    public string Name { get; } = name;
}

class FileSystemFile(string name, long size) : FileSystemItem(name)
{
    public long Size { get; } = size;
}

class FileSystemDir(string name, FileSystemDir? parent) : FileSystemItem(name)
{
    public List<FileSystemItem> Children { get; } = [];
    public long CalculatedSize { get; set; }
    public FileSystemDir? Parent { get; } = parent;
}
