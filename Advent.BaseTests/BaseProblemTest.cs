using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent.Common;

public abstract class BaseProblemTest
{
    protected static void Test<TR>(int year, int number, IProblemSolver<TR> solver, string filename, bool first, TR result)
    {
        var fullpath = GetPath(year, number, filename, first);

        try
        {
            var actual = first
                ? solver.RunA(fullpath)
                : solver.RunB(fullpath);

            Assert.AreEqual(result, actual);
        }
        catch (NotImplementedException)
        {
            Assert.Inconclusive("Not implemented");
        }
    }

    protected static void Test<TRA, TRB>(int year, int number, IProblemSolver<TRA, TRB> solver, string filename, bool first, TRA resultA, TRB resultB)
    {
        var fullpath = GetPath(year, number, filename, first);

        try
        {
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
        catch (NotImplementedException)
        {
            Assert.Inconclusive("Not implemented");
        }
    }

    static string GetPath(int year, int number, string filename, bool first)
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
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ProblemTestAttribute<TR> : TestMethodAttribute, ITestDataSource
{
    readonly TR sampleA;
    readonly TR resultA;
    readonly TR sampleB = default!;
    readonly TR resultB = default!;

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

    public string GetDisplayName(MethodInfo methodInfo, object?[]? data)
    {
        var filename = (string)data![0]!;
        var first = (bool)data![1]!;

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

    public string GetDisplayName(MethodInfo methodInfo, object?[]? data)
    {
        var filename = (string)data![0]!;
        var first = (bool)data![1]!;

        return $"{(first ? "RunA" : "RunB")} {filename}";
    }
}
