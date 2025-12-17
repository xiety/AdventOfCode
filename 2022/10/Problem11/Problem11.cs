using System.Linq.Expressions;
using System.Numerics;
using System.Text.RegularExpressions;

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

        foreach (var _ in totalRounds)
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

    Func<T, T> compiledCalculate = null!;

    public void Initialize()
        => compiledCalculate = GenerateFunction();

    Func<T, T> GenerateFunction()
    {
        var old = Expression.Parameter(typeof(T), "old");

        var left = OperationLeft switch
        {
            "old" => (Expression)old,
            string => Expression.Constant(T.Parse(OperationLeft, null), typeof(T)),
        };

        var right = OperationRight switch
        {
            "old" => (Expression)old,
            string => Expression.Constant(T.Parse(OperationRight, null), typeof(T)),
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
        Monkey (?<{nameof(Monkey<>.Number)}>\d+):
          Starting items: (?<{nameof(Monkey<>.StartingItems)}>\d+)(?:, (?<{nameof(Monkey<>.StartingItems)}>\d+))*
          Operation: new = (?<{nameof(Monkey<>.OperationLeft)}>.*) (?<{nameof(Monkey<>.OperationOperator)}>.) (?<{nameof(Monkey<>.OperationRight)}>.*)
          Test: divisible by (?<{nameof(Monkey<>.Test)}>.*)
            If true: throw to monkey (?<{nameof(Monkey<>.IfTrue)}>.*)
            If false: throw to monkey (?<{nameof(Monkey<>.IfFalse)}>.*)
        """)]
    public static partial Regex Regex();
}
