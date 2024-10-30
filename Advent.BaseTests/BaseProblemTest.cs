using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent.Common;

public abstract class BaseProblemTest
{
    protected static void Test<TR>(int year, int number, ISolver<TR> solver, bool isA, bool isSample, TR result)
    {
        var fullpath = GetPath(year, number, isA, isSample);

        var text = File.ReadAllText(fullpath);

        var actual = isA
            ? solver.RunA(text, isSample)
            : solver.RunB(text, isSample);

        Assert.AreEqual(result, actual);
    }

    protected static void Test<TR>(int year, int number, IProblemSolver<TR> solver, string filename, bool first, TR result)
    {
        var fullpath = GetPath(year, number, filename, first);

        var actual = first
            ? solver.RunA(fullpath)
            : solver.RunB(fullpath);

        Assert.AreEqual(result, actual);
    }

    protected static void Test<TRA, TRB>(int year, int number, IProblemSolver<TRA, TRB> solver, string filename, bool first, TRA resultA, TRB resultB)
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
        var folder = GetFolder(year, number);

        var fullpath = Path.Combine(folder, filename);

        if (!File.Exists(fullpath))
        {
            var name = Path.GetFileNameWithoutExtension(filename);
            fullpath = Path.Combine(folder, $"{name}{(first ? "A" : "B")}.txt");
        }

        return fullpath;
    }

    private static string GetPath(int year, int number, bool isA, bool isSample)
    {
        var folder = GetFolder(year, number);

        var filename = isSample ? "sample.txt" : "input.txt";

        var fullpath = Path.Combine(folder, filename);

        if (!File.Exists(fullpath))
        {
            var name = Path.GetFileNameWithoutExtension(filename);
            fullpath = Path.Combine(folder, $"{name}{(isA ? "A" : "B")}.txt");
        }

        return fullpath;
    }

    protected static string GetFolder(int year, int number)
        => @$"..\..\..\..\A{year:0000}.Problem{number:00}\Data\";
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ProblemTestAttribute<TR> : TestMethodAttribute, ITestDataSource
{
    private readonly TR sampleA;
    private readonly TR resultA;
    private readonly TR sampleB = default!;
    private readonly TR resultB = default!;

    public ProblemTestAttribute(TR sampleA, TR resultA, TR sampleB, TR resultB)
    {
        this.sampleA = sampleA;
        this.resultA = resultA;
        this.sampleB = sampleB;
        this.resultB = resultB;
    }

    public ProblemTestAttribute(TR sampleA, TR resultA)
    {
        this.sampleA = sampleA;
        this.resultA = resultA;
    }

    public IEnumerable<object?[]> GetData(MethodInfo methodInfo)
    {
        var list = new List<object?[]>
        {
            (["sample.txt", true, sampleA]),
            (["input.txt", true, resultA]),
            (["sample.txt", false, sampleB]),
            (["input.txt", false, resultB])
        };

        return list;
    }

    public string? GetDisplayName(MethodInfo methodInfo, object?[]? data)
    {
        var filename = (string)data![0]!;
        var first = (bool)data![1]!;
        var value = (TR)data![2]!;

        return $"{(first ? "RunA" : "RunB")} {filename}";
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

        return $"{(first ? "RunA" : "RunB")} {filename}";
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ProblemDataAttribute<TR>(TR sampleA, TR resultA, TR sampleB, TR resultB) : TestMethodAttribute, ITestDataSource
{
    public IEnumerable<object?[]> GetData(MethodInfo methodInfo)
    {

        yield return [true, true, sampleA];
        yield return [true, false, resultA];
        yield return [false, true, sampleB];
        yield return [false, false, resultB];
    }

    public string? GetDisplayName(MethodInfo methodInfo, object?[]? data)
    {
        if (data is [bool isA, bool isSample, TR value])
        {
            var method = isA ? nameof(ISolver<int>.RunA) : nameof(ISolver<int>.RunB);
            var mode = isSample ? "sample" : "input";
            return $"{method} {mode}";
        }

        throw new();
    }
}
