using Advent.Common;

namespace A2022.Problem07;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var root = LoadFile(filename);

        var flatternDirs = Flattern(root).OfType<FileSystemDir>();

        var maximumSize = 100000;

        var result = flatternDirs
            .Where(a => a != root)
            .Where(a => a.CalculatedSize < maximumSize)
            .Sum(a => a.CalculatedSize);

        return result;
    }

    public long RunB(string filename)
    {
        var root = LoadFile(filename);

        var flatternDirs = Flattern(root).OfType<FileSystemDir>();

        var totalSpace = 70_000_000;
        var freeSpaceRequired = 30_000_000;

        var currentFreeSpace = totalSpace - root.CalculatedSize;
        var needToFree = freeSpaceRequired - currentFreeSpace;

        var ordered = flatternDirs
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

    static IEnumerable<FileSystemItem> Flattern(FileSystemDir parent)
    {
        foreach (var child in parent.Children)
            yield return child;

        foreach (var child in parent.Children.OfType<FileSystemDir>())
            foreach (var item in Flattern(child))
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
                    if (currentDir.Parent is null)
                        throw new Exception("Parent is null");

                    currentDir = currentDir.Parent;
                }
                else
                {
                    var existing = currentDir.Children
                        .OfType<FileSystemDir>()
                        .FirstOrDefault(a => a.Name == cd.Dir);

                    if (existing is not null)
                    {
                        currentDir = existing;
                    }
                    else
                    {
                        existing = new FileSystemDir(cd.Dir, currentDir);
                        currentDir.Children.Add(existing);
                        currentDir = existing;
                    }
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
        var dirPrefix = "dir ";

        if (line.StartsWith(dirPrefix))
            return new CommandLsDir(line[dirPrefix.Length..]);

        var n = line.IndexOf(' ');

        return new CommandLsFile(line[(n + 1)..], long.Parse(line[0..n]));
    }
}

abstract record Command();
record CommandCd(string Dir) : Command;
record CommandLs(CommandLsItem[] Output) : Command;
abstract record CommandLsItem(string Name);
record CommandLsFile(string Name, long Size) : CommandLsItem(Name);
record CommandLsDir(string Name) : CommandLsItem(Name);

record FileSystemItem(string Name);
record FileSystemFile(string Name, long Size) : FileSystemItem(Name);

record FileSystemDir(string Name, FileSystemDir? Parent) : FileSystemItem(Name)
{
    public List<FileSystemItem> Children { get; } = [];
    public long CalculatedSize { get; set; }
}
