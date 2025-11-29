namespace Advent.Common;

[AttributeUsage(AttributeTargets.Method)]
public sealed class GeneratedTestAttribute<T>(T sample, T input) : Attribute
{
    public T Sample { get; } = sample;
    public T Input { get; } = input;
}

public abstract class GeneratedTestBase
{
    protected static string[] LoadInput(string sourceFilePath, bool isPartA, bool isSample)
    {
        var sourceDir = Path.GetDirectoryName(sourceFilePath)
            ?? throw new DirectoryNotFoundException($"Could not determine directory of {sourceFilePath}");

        var dataFolder = Path.Combine(sourceDir, "Data");

        var filename = isSample ? "sample.txt" : "input.txt";
        var fullPath = Path.Combine(dataFolder, filename);

        if (!File.Exists(fullPath))
        {
            var name = Path.GetFileNameWithoutExtension(filename);
            fullPath = Path.Combine(dataFolder, $"{name}{(isPartA ? "A" : "B")}.txt");
        }

        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"Input file not found: {fullPath}");

        return File.ReadAllLines(fullPath);
    }
}
