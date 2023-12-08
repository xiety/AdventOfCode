using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent.Common;

public abstract class BaseProblemTest
{
    protected void Test<TR>(int year, int number, IProblemSolver<TR> solver, string filename, bool first, TR result)
    {
        var fullpath = GetPath(year, number, filename, first);

        var actual = first
            ? solver.RunA(fullpath)
            : solver.RunB(fullpath);

        Assert.AreEqual(result, actual);
    }

    protected void Test<TRA, TRB>(int year, int number, IProblemSolver<TRA, TRB> solver, string filename, bool first, TRA resultA, TRB resultB)
    {
        var fullpath = GetPath(year, number, filename, first);

        if (first)
        {
            var actual = solver.RunA(fullpath);
            Assert.AreEqual(resultA, actual);
        }
        else
        {
            var actual = solver.RunB(fullpath);
            Assert.AreEqual(resultB, actual);
        }
    }

    private static string GetPath(int year, int number, string filename, bool first)
    {
        var folder = @$"..\..\..\..\A{year:0000}.Problem{number:00}\Data\";

        var fullpath = Path.Combine(folder, filename);

        if (filename == "sample.txt")
        {
            if (!File.Exists(fullpath))
                fullpath = Path.Combine(folder, $"sample{(first ? "A" : "B")}.txt");
        }

        return fullpath;
    }

    public static string? TrimText(string? text)
        => text
            ?.Replace("\r\n", " ")
            ?.TrimLength(9);
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

        var text = BaseProblemTest.TrimText(value?.ToString());

        return $"{(first ? "RunA" : "RunB")} {filename} {text}";
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ProblemTestAttribute<TRA, TRB>(TRA sampleA, TRA resultA, TRB sampleB, TRB resultB) : TestMethodAttribute, ITestDataSource
{
    public IEnumerable<object?[]> GetData(MethodInfo methodInfo)
    {
        return [
            ["sample.txt", true, sampleA, default(TRB)],
            ["input.txt", true, resultA, default(TRB)],
            ["sample.txt", false, default(TRA), sampleB],
            ["input.txt", false, default(TRA), resultB],
        ];
    }

    public string? GetDisplayName(MethodInfo methodInfo, object?[]? data)
    {
        var filename = (string)data![0]!;
        var first = (bool)data![1]!;
        var valueA = data![2];
        var valueB = data![3];

        var text = BaseProblemTest.TrimText(first ? valueA?.ToString() : valueB?.ToString());

        return $"{(first ? "RunA" : "RunB")} {filename} {text}";
    }
}
