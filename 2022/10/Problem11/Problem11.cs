using System.Linq.Expressions;
using System.Numerics;
using System.Text.RegularExpressions;

using Advent.Common;

namespace A2022.Problem11;

public static class Solver
{
    [GeneratedTest<long>(10605, 50616)]
    public static long RunA(string[] lines)
        => Run<decimal>(lines, (_, value) => decimal.Floor(value / 3), 20);

    [GeneratedTest<long>(2713310158, 11309046332)]
    public static long RunB(string[] lines)
        => Run<decimal>(lines, (multi, value) => value % multi, 10000);

    static long Run<T>(string[] lines, Func<T, T, T> modifyFunc, int totalRounds)
        where T : INumber<T>
    {
        var monkeys = LoadData<T>(lines);

        var multi = monkeys.Select(a => a.Test).Mul();

        foreach (var round in totalRounds)
        {
            foreach (var monkey in monkeys)
            {
                monkey.StepsCount += monkey.StartingItems.Count;

                foreach (var item in monkey.StartingItems)
                {
                    var value = monkey.Calculate(item);

                    value = modifyFunc(multi, value);

                    var newMonkeyNumber = monkey.Check(value) ? monkey.IfTrue : monkey.IfFalse;

                    var newMonkey = monkeys[newMonkeyNumber];
                    newMonkey.StartingItems.Add(value);
                }

                monkey.StartingItems.Clear();
            }
        }

        return monkeys
            .OrderByDescending(a => a.StepsCount)
            .Take(2)
            .Select(a => a.StepsCount)
            .MulLong();
    }

    static Monkey<T>[] LoadData<T>(string[] lines)
        where T : IParsable<T>, INumber<T>
    {
        var text = String.Join(Environment.NewLine, lines);

        return text
            .Split(Environment.NewLine + Environment.NewLine)
            .ToArray(CompiledRegs.Regex().MapTo<Monkey<T>>);
    }
}

record Monkey<T>(
    int Number,
    List<T> StartingItems,
    string OperationLeft,
    string OperationOperator,
    string OperationRight,
    T Test,
    int IfTrue,
    int IfFalse
) : IInitializable
    where T : IParsable<T>, INumber<T>
{
    public int StepsCount { get; set; }

    Func<T, T> compiledCalculate = default!;

    public void Initialize()
        => compiledCalculate = GenerateFunction();

    Func<T, T> GenerateFunction()
    {
        var old = Expression.Parameter(typeof(T), "old");

        var left = OperationLeft switch
        {
            "old" => (Expression)old,
            string a => Expression.Constant(T.Parse(a, null), typeof(T)),
        };

        var right = OperationRight switch
        {
            "old" => (Expression)old,
            string a => Expression.Constant(T.Parse(a, null), typeof(T)),
        };

        var operation = OperationOperator switch
        {
            "*" => Expression.Multiply(left, right),
            "/" => Expression.Divide(left, right),
            "+" => Expression.Add(left, right),
            "-" => Expression.Subtract(left, right),
        };

        var expression = Expression.Lambda(operation, old);
        return (Func<T, T>)expression.Compile();
    }

    public T Calculate(T value)
        => compiledCalculate(value);

    public bool Check(T value)
        => (value % Test) == T.Zero;
}

static partial class CompiledRegs
{
    [GeneratedRegex($"""
        Monkey (?<{nameof(Monkey<int>.Number)}>\d+):
          Starting items: (?<{nameof(Monkey<int>.StartingItems)}>\d+)(?:, (?<{nameof(Monkey<int>.StartingItems)}>\d+))*
          Operation: new = (?<{nameof(Monkey<int>.OperationLeft)}>.*) (?<{nameof(Monkey<int>.OperationOperator)}>.) (?<{nameof(Monkey<int>.OperationRight)}>.*)
          Test: divisible by (?<{nameof(Monkey<int>.Test)}>.*)
            If true: throw to monkey (?<{nameof(Monkey<int>.IfTrue)}>.*)
            If false: throw to monkey (?<{nameof(Monkey<int>.IfFalse)}>.*)
        """)]
    public static partial Regex Regex();
}
