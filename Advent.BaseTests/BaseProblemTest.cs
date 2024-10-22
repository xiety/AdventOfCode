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
        var folder = GetFolder(year, number);

        var fullpath = Path.Combine(folder, filename);

        if (!File.Exists(fullpath))
        {
            var name = Path.GetFileNameWithoutExtension(filename);
            fullpath = Path.Combine(folder, $"{name}{(first ? "A" : "B")}.txt");
        }

        return fullpath;
    }

    protected static string GetFolder(int year, int number)
        => @$"..\..\..\..\A{year:0000}.Problem{number:00}\Data\";

    public static string? TrimText(string? text)
        => text
            ?.Replace("\r\n", " ")
            ?.TrimLength(9);
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ProblemTestAttribute<TR> : TestMethodAttribute, ITestDataSource
{
    private readonly TR sampleA;
    private readonly TR resultA;
    private readonly TR sampleB = default!;
    private readonly TR resultB = default!;

    private readonly bool hasB;

    public ProblemTestAttribute(TR sampleA, TR resultA, TR sampleB, TR resultB)
    {
        this.sampleA = sampleA;
        this.resultA = resultA;
        this.sampleB = sampleB;
        this.resultB = resultB;
        hasB = true;
    }

    public ProblemTestAttribute(TR sampleA, TR resultA)
    {
        this.sampleA = sampleA;
        this.resultA = resultA;
        hasB = false;
    }

    public IEnumerable<object?[]> GetData(MethodInfo methodInfo)
    {
        var list = new List<object?[]>();
        list.Add(["sample.txt", true, sampleA]);
        list.Add(["input.txt", true, resultA]);

        if (hasB)
        {
            list.Add(["sample.txt", false, sampleB]);
            list.Add(["input.txt", false, resultB]);
        }

        return list;
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
