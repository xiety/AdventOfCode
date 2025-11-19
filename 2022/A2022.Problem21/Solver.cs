using System.Numerics;
using System.Text.RegularExpressions;

using Advent.Common;

namespace A2022.Problem21;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var monkeys = LoadFile(filename);
        var root = (MonkeyOperation)monkeys.First(a => a.Name == "root");
        var result = CalculateRecurse(root);
        return (long)result;
    }

    public long RunB(string filename)
    {
        var monkeys = LoadFile(filename);

        var root = (MonkeyOperation)monkeys.First(a => a.Name == "root");
        var humn = (MonkeyValue)monkeys.First(a => a.Name == "humn");

        var (ourSide, theirSide) = FindSide(root, humn);
        var otherSideResult = CalculateRecurse(theirSide);
        var result = CalculateInverse(ourSide, humn, otherSideResult);

        return (long)result;
    }

    static BigInteger CalculateInverse(Monkey resultMonkey, Monkey variableMonkey, BigInteger awaitingResult)
    {
        switch (resultMonkey)
        {
            case MonkeyOperation mo:
                var (ourSide, theirSide) = FindSide(mo, variableMonkey);

                var isLeft = ourSide == mo.Left;

                var otherSideResult = CalculateRecurse(theirSide);

                var outSideResult = (mo.Operation, isLeft) switch
                {
                    ("+", _) => awaitingResult - otherSideResult,

                    ("-", true) => awaitingResult + otherSideResult,
                    ("-", false) => otherSideResult - awaitingResult,

                    ("*", _) => awaitingResult / otherSideResult,

                    ("/", true) => awaitingResult * otherSideResult,
                    ("/", false) => otherSideResult / awaitingResult,
                };

                return CalculateInverse(ourSide, variableMonkey, outSideResult);

            case MonkeyValue mv when mv == variableMonkey:
                return awaitingResult;
        }

        throw new();
    }

    static BigInteger CalculateRecurse(Monkey monkey)
    {
        switch (monkey)
        {
            case MonkeyOperation mo:
                if (mo.Result is BigInteger r)
                    return r;

                var left = CalculateRecurse(mo.Left!);
                var right = CalculateRecurse(mo.Right!);

                checked
                {
                    mo.Result = mo.Operation switch
                    {
                        "+" => left + right,
                        "-" => left - right,
                        "*" => left * right,
                        "/" => left / right,
                    };
                }

                return mo.Result.Value;

            case MonkeyValue mv:
                return mv.Value;
        }

        throw new();
    }

    static (Monkey, Monkey) FindSide(MonkeyOperation mo, Monkey search)
    {
        if (Contains(mo.Left!, search))
            return (mo.Left!, mo.Right!);

        if (Contains(mo.Right!, search))
            return (mo.Right!, mo.Left!);

        throw new();
    }

    static bool Contains(Monkey monkey, Monkey search)
    {
        if (monkey == search)
            return true;

        if (monkey is MonkeyOperation mo)
        {
            var result = false;

            if (!result)
                result = Contains(mo.Left!, search);

            if (!result)
                result = Contains(mo.Right!, search);

            return result;
        }

        return false;
    }

    static Monkey[] LoadFile(string filename)
    {
        var monkeys = File.ReadAllLines(filename)
            .ToArray(a => a.MapTo<Monkey, MonkeyOperation, MonkeyValue>(
                CompiledRegs.RegexOperation(), CompiledRegs.RegexValue()));

        foreach (var monkey in monkeys.OfType<MonkeyOperation>())
        {
            monkey.Left = monkeys.First(a => a.Name == monkey.LeftName);
            monkey.Right = monkeys.First(a => a.Name == monkey.RightName);
        }

        return monkeys;
    }
}

abstract class Monkey(string name)
{
    public string Name { get; } = name;
}

class MonkeyOperation : Monkey
{
    public string LeftName { get; }
    public string RightName { get; }
    public string Operation { get; }

    public Monkey? Left { get; set; }
    public Monkey? Right { get; set; }
    public BigInteger? Result { get; set; }

    public MonkeyOperation(string name, string leftName, string rightName, string operation) : base(name)
        => (LeftName, RightName, Operation) = (leftName, rightName, operation);
}

class MonkeyValue(string name, int value) : Monkey(name)
{
    public BigInteger Value { get; set; } = value;
}

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(MonkeyOperation.Name)}>[a-z]{{4}}): (?<{nameof(MonkeyOperation.LeftName)}>[a-z]{{4}}) (?<{nameof(MonkeyOperation.Operation)}>.) (?<{nameof(MonkeyOperation.RightName)}>[a-z]{{4}})")]
    public static partial Regex RegexOperation();

    [GeneratedRegex(@$"^(?<{nameof(MonkeyValue.Name)}>[a-z]{{4}}): (?<{nameof(MonkeyValue.Value)}>\d+)")]
    public static partial Regex RegexValue();
}
