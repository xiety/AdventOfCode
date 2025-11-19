using System.Text.RegularExpressions;

using Advent.Common;

namespace A2022.Problem10;

public class Solver : IProblemSolver<int, string>
{
    public int RunA(string filename)
    {
        var commands = LoadFile(filename);

        var currentCycle = 0;
        var total = 0;
        var x = 1;

        foreach (var command in commands)
        {
            switch (command)
            {
                case CommandNoop:
                    currentCycle++;
                    total += Check(currentCycle, x);
                    break;

                case CommandAddx addx:
                    for (var i = 0; i < 2; ++i)
                    {
                        currentCycle++;
                        total += Check(currentCycle, x);
                    }

                    x += addx.V;
                    break;
            }
        }

        return total;
    }

    public string RunB(string filename)
    {
        var commands = LoadFile(filename);

        var x = 1;
        var currentCycle = 0;
        var screen = new char[40, 8];

        foreach (var command in commands)
        {
            switch (command)
            {
                case CommandNoop:
                    currentCycle++;
                    Draw(screen, currentCycle, x);
                    break;

                case CommandAddx addx:
                    currentCycle++;
                    Draw(screen, currentCycle, x);

                    currentCycle++;
                    Draw(screen, currentCycle, x);

                    x += addx.V;
                    break;
            }
        }

        return screen.ToString(Environment.NewLine, "", a => a == 0 ? " " : $"{a}")
            .TrimEnd();
    }

    static int Check(int currentCycle, int x)
    {
        int[] importantCycleNumbers = [20, 60, 100, 140, 180, 220];

        return importantCycleNumbers.Contains(currentCycle) ? x * currentCycle : 0;
    }

    static void Draw(char[,] screen, int currentCycle, int x)
    {
        var posX = (currentCycle - 1) % 40;
        var posY = (currentCycle - 1) / 40;

        screen[posX, posY] = (x - 1 <= posX && x + 1 >= posX) ? '#' : '.';
    }

    static IEnumerable<Command> LoadFile(string fileName)
    {
#pragma warning disable RCS1124 // Inline local variable
        var lines = File.ReadAllLines(fileName);
#pragma warning restore RCS1124 // Inline local variable

        foreach (var item in lines)
        {
            if (item is "noop")
            {
                yield return new CommandNoop();
            }
            else
            {
                var addx = CompiledRegs.RegexAddx().MapTo<ParsedAddx>(item);

                yield return new CommandAddx(addx.V);
            }
        }
    }
}

record Command;
record CommandNoop : Command;
record CommandAddx(int V) : Command;

record ParsedAddx(int V);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"addx (?<{nameof(ParsedAddx.V)}>-?\d+)")]
    public static partial Regex RegexAddx();
}
