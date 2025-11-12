using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Advent.Common;

public abstract class FolderSolverTest : BaseSolverTest
{
    protected override string GetFolder(int year, int number)
        => @$"..\..\..\..\Data\Problem{number:00}\";
}

public abstract class BaseSolverTest
{
    protected void Test<TR>(ISolver<TR> solver, ITestParameter p)
    {
        var (year, number) = Parse(solver.GetType());

        if (p is TestParameter<TR> tp)
            Test(year, number, tp, lines => tp.IsA ? solver.RunA(lines, tp.IsSample) : solver.RunB(lines, tp.IsSample));
        else
            throw new($"Type of p is {p.GetType()} and TR is {typeof(TR)}");
    }

    protected void Test<TRA, TRB>(ISolver<TRA, TRB> solver, ITestParameter p)
    {
        var (year, number) = Parse(solver.GetType());

        if (p is TestParameter<TRA> tpa)
            Test(year, number, tpa, lines => solver.RunA(lines, tpa.IsSample));
        else if (p is TestParameter<TRB> tpb)
            Test(year, number, tpb, lines => solver.RunB(lines, tpb.IsSample));
        else
            throw new();
    }

    static (int, int) Parse(Type type)
    {
        var ns = type.Namespace!;
        var year = int.Parse(ns[1..5]);
        var number = int.Parse(ns[13..15]);
        return (year, number);
    }

    protected void Test<TR>(int year, int number, TestParameter<TR> p, Func<string[], TR> execute)
    {
        try
        {
            var fullpath = GetPath(year, number, p.IsA, p.IsSample);
            var lines = File.ReadAllLines(fullpath);
            var actual = execute(lines);
            Assert.AreEqual(p.Value, actual);
        }
        catch (NotImplementedException)
        {
            Assert.Inconclusive("Not implemented");
        }
    }

    string GetPath(int year, int number, bool isA, bool isSample)
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

    protected virtual string GetFolder(int year, int number)
        => @$"..\..\..\..\A{year:0000}.Problem{number:00}\Data\";
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ProblemDataAttribute<TR>(TR sampleA, TR resultA, TR sampleB, TR resultB, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
    : TestMethodAttribute(callerFilePath, callerLineNumber), ITestDataSource
{
    public IEnumerable<object?[]> GetData(MethodInfo methodInfo)
    {
        yield return [new TestParameter<TR>(true, true, sampleA)];
        yield return [new TestParameter<TR>(true, false, resultA)];
        yield return [new TestParameter<TR>(false, true, sampleB)];
        yield return [new TestParameter<TR>(false, false, resultB)];
    }

    public string GetDisplayName(MethodInfo methodInfo, object?[]? data)
    {
        if (data is [TestParameter<TR> tp])
        {
            var method = tp.IsA ? nameof(ISolver<>.RunA) : nameof(ISolver<>.RunB);
            var mode = tp.IsSample ? "sample" : "input";
            return $"{method} {mode}";
        }

        throw new();
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ProblemDataAttribute<TRA, TRB>(TRA sampleA, TRA resultA, TRB sampleB, TRB resultB, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
    : TestMethodAttribute(callerFilePath, callerLineNumber), ITestDataSource
{
    public IEnumerable<object?[]> GetData(MethodInfo methodInfo)
    {
        yield return [new TestParameter<TRA>(true, true, sampleA)];
        yield return [new TestParameter<TRA>(true, false, resultA)];
        yield return [new TestParameter<TRB>(false, true, sampleB)];
        yield return [new TestParameter<TRB>(false, false, resultB)];
    }

    public string GetDisplayName(MethodInfo methodInfo, object?[]? data)
    {
        switch (data)
        {
            case [TestParameter<TRA> tpa]:
                {
                    const string method = nameof(ISolver<>.RunA);
                    var mode = tpa.IsSample ? "sample" : "input";
                    return $"{method} {mode}";
                }

            case [TestParameter<TRB> tpb]:
                {
                    const string method = nameof(ISolver<>.RunB);
                    var mode = tpb.IsSample ? "sample" : "input";
                    return $"{method} {mode}";
                }

            default:
                throw new();
        }
    }
}

public interface ITestParameter;

[DataContract]
public record TestParameter<TR>(
    [property: DataMember] bool IsA,
    [property: DataMember] bool IsSample,
    [property: DataMember] TR Value
) : ITestParameter;
