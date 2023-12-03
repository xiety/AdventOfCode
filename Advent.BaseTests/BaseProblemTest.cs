using System.Diagnostics;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent.Common;

public abstract class BaseProblemTest
{
    protected void Test<TR>(int year, int number, IProblemSolver<TR> solver, string filename, bool first, TR result)
    {
        var folder = @$"..\..\..\..\A{year:0000}.Problem{number:00}\Data\";

        var fullpath = Path.Combine(folder, filename);

        if (filename == "sample.txt")
        {
            if (!File.Exists(fullpath))
                fullpath = Path.Combine(folder, $"sample{(first ? "A" : "B")}.txt");
        }

        var actual = first
            ? solver.RunA(fullpath)
            : solver.RunB(fullpath);

        Assert.AreEqual(result, actual);
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ProblemTestAttribute<TR>(TR sampleA, TR resultA, TR sampleB, TR resultB) : TestMethodAttribute, ITestDataSource
{
    public IEnumerable<object?[]> GetData(MethodInfo methodInfo)
    {
        return [
            ["sample.txt", true, sampleA],
            ["input.txt", true, resultA],
            ["sample.txt", false, sampleB],
            ["input.txt", false, resultB],
        ];
    }

    public string? GetDisplayName(MethodInfo methodInfo, object?[]? data)
    {
        var filename = (string)data![0]!;
        var first = (bool)data![1]!;
        var value = (TR)data![2]!;

        return $"{(first ? "RunA" : "RunB")} {filename} {value}";
    }
}
