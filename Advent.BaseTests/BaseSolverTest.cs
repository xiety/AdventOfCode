using System.Reflection;
using System.Runtime.Serialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent.Common;

public abstract class BaseSolverTest
{
    protected static void Test<TR>(ISolver<TR> solver, ITestParameter p)
    {
        var (year, number) = Parse(solver.GetType());

        if (p is TestParameter<TR> tp)
            Test(year, number, tp, lines => tp.IsA ? solver.RunA(lines, tp.IsSample) : solver.RunB(lines, tp.IsSample));
        else
            throw new();
    }

    protected static void Test<TRA, TRB>(ISolver<TRA, TRB> solver, ITestParameter p)
    {
        var (year, number) = Parse(solver.GetType());

        if (p is TestParameter<TRA> tpa)
            Test(year, number, tpa, lines => solver.RunA(lines, tpa.IsSample));
        else if (p is TestParameter<TRB> tpb)
            Test(year, number, tpb, lines => solver.RunB(lines, tpb.IsSample));
        else
            throw new();
    }

    private static (int, int) Parse(Type type)
    {
        var ns = type.Namespace!;
        var year = int.Parse(ns[1..5]);
        var number = int.Parse(ns[13..15]);
        return (year, number);
    }

    protected static void Test<TR>(int year, int number, TestParameter<TR> p, Func<string[], TR> execute)
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
public class ProblemDataAttribute<TR>(TR sampleA, TR resultA, TR sampleB, TR resultB) : TestMethodAttribute, ITestDataSource
{
    public IEnumerable<object?[]> GetData(MethodInfo methodInfo)
    {
        yield return [new TestParameter<TR>(true, true, sampleA)];
        yield return [new TestParameter<TR>(true, false, resultA)];
        yield return [new TestParameter<TR>(false, true, sampleB)];
        yield return [new TestParameter<TR>(false, false, resultB)];
    }

    public string? GetDisplayName(MethodInfo methodInfo, object?[]? data)
    {
        if (data is [TestParameter<TR> tp])
        {
            var method = tp.IsA ? nameof(ISolver<int>.RunA) : nameof(ISolver<int>.RunB);
            var mode = tp.IsSample ? "sample" : "input";
            return $"{method} {mode}";
        }

        throw new();
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ProblemDataAttribute<TRA, TRB>(TRA sampleA, TRA resultA, TRB sampleB, TRB resultB) : TestMethodAttribute, ITestDataSource
{
    public IEnumerable<object?[]> GetData(MethodInfo methodInfo)
    {
        yield return [new TestParameter<TRA>(true, true, sampleA)];
        yield return [new TestParameter<TRA>(true, false, resultA)];
        yield return [new TestParameter<TRB>(false, true, sampleB)];
        yield return [new TestParameter<TRB>(false, false, resultB)];
    }

    public string? GetDisplayName(MethodInfo methodInfo, object?[]? data)
    {
        if (data is [TestParameter<TRA> tpa])
        {
            var method = nameof(ISolver<int>.RunA);
            var mode = tpa.IsSample ? "sample" : "input";
            return $"{method} {mode}";
        }
        else if (data is [TestParameter<TRB> tpb])
        {
            var method = nameof(ISolver<int>.RunB);
            var mode = tpb.IsSample ? "sample" : "input";
            return $"{method} {mode}";
        }

        throw new();
    }
}

public interface ITestParameter;

[DataContract]
public record TestParameter<TR>(
    [property: DataMember] bool IsA,
    [property: DataMember] bool IsSample,
    [property: DataMember] TR Value
) : ITestParameter;
