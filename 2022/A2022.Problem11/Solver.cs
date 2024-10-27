using System.Linq.Expressions;
using System.Numerics;
using System.Text.RegularExpressions;

using Advent.Common;

namespace A2022.Problem11;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
        => Run<decimal>(filename, (_, value) => decimal.Floor(value / 3), 20);

    public long RunB(string filename)
        => Run<decimal>(filename, (multi, value) => value % multi, 10000);

    static long Run<T>(string filename, Func<T, T, T> modifyFunc, int totalRounds)
        where T : INumber<T>
    {
        var monkeys = LoadFile<T>(filename).ToArray();

        var multi = monkeys.Select(a => a.Test).Mul();

        for (var round = 0; round < totalRounds; ++round)
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

        var result = monkeys
            .OrderByDescending(a => a.StepsCount)
            .Take(2)
            .Select(a => a.StepsCount)
            .MulLong();

        return result;
    }

    static IEnumerable<Monkey<T>> LoadFile<T>(string fileName)
        where T : IParsable<T>, INumber<T>
    {
        var text = File.ReadAllText(fileName);

        var items = text.ReplaceLineEndings()
            .Split(Environment.NewLine + Environment.NewLine)
            .Select(CompiledRegs.Regex().MapTo<Monkey<T>>);

        return items;
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

    private Func<T, T> compiledCalculate = default!;

    public void Initialize()
        => compiledCalculate = GenerateFunction();

    private Func<T, T> GenerateFunction()
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
            string a => throw new NotSupportedException(a)
        };

        var expression = Expression.Lambda(operation, old);
        var func = (Func<T, T>)expression.Compile();

        return func;
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
