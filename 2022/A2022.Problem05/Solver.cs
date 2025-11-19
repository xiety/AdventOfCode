using System.Text.RegularExpressions;

using Advent.Common;

namespace A2022.Problem05;

public class Solver : IProblemSolver<string>
{
    public string RunA(string filename)
    {
        var (crates, commands) = LoadFile(filename);

        foreach (var command in commands)
            for (var p = 0; p < command.Quantity; ++p)
                crates[command.To].Push(crates[command.From].Pop());

        return CollectLetters(crates);
    }

    public string RunB(string filename)
    {
        var (crates, commands) = LoadFile(filename);

        foreach (var command in commands)
            foreach (var crate in crates[command.From].PopMultiple(command.Quantity).Reverse())
                crates[command.To].Push(crate);

        return CollectLetters(crates);
    }

    static string CollectLetters(Stack<char>[] crates)
        => new(Enumerable.Range(1, crates.Length - 1).ToArray(a => crates[a].Peek()));

    static (Stack<char>[] crates, IEnumerable<Item> commands) LoadFile(string filename)
    {
        var lines = File.ReadAllLines(filename);
        var parts = lines.SplitBy(String.Empty).ToArray();

        var cratesLines = parts[0].ToArray();
        var last = cratesLines[^1];
        var numberOfColumns = (last.Length + 1) / 4;
        var crates = new Stack<char>[numberOfColumns + 1];

        for (var i = 0; i < numberOfColumns; ++i)
        {
            crates[i + 1] = [];

            for (var j = 0; j < cratesLines.Length - 1; ++j)
            {
                var letter = cratesLines[cratesLines.Length - 2 - j][i * 4 + 1];

                if (letter is not ' ')
                    crates[i + 1].Push(letter);
            }
        }

        var commands = parts[1]
            .Select(CompiledRegs.Line().MapTo<Item>);

        return (crates, commands);
    }
}

record Item(int Quantity, int From, int To);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"move (?<{nameof(Item.Quantity)}>\d*) from (?<{nameof(Item.From)}>\d*) to (?<{nameof(Item.To)}>\d*)")]
    public static partial Regex Line();
}
