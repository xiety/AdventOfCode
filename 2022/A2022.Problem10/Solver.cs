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
        var X = 1;

        foreach (var command in commands)
        {
            switch (command)
            {
                case CommandNoop:
                    currentCycle++;
                    total += Check(currentCycle, X);
                    break;

                case CommandAddx addx:
                    for (var i = 0; i < 2; ++i)
                    {
                        currentCycle++;
                        total += Check(currentCycle, X);
                    }

                    X += addx.V;
                    break;
            }
        }

        return total;
    }

    public string RunB(string filename)
    {
        var commands = LoadFile(filename);

        var X = 1;
        var currentCycle = 0;
        var screen = new char[40, 8];

        foreach (var command in commands)
        {
            if (command is CommandNoop)
            {
                currentCycle++;
                Draw(screen, currentCycle, X);
            }
            else if (command is CommandAddx addx)
            {
                currentCycle++;
                Draw(screen, currentCycle, X);

                currentCycle++;
                Draw(screen, currentCycle, X);

                X += addx.V;
            }
        }

        return screen.ToString(Environment.NewLine, "", a => a == 0 ? " " : $"{a}")
            .TrimEnd();
    }

    static int Check(int currentCycle, int X)
    {
        int[] importantCycleNumbers = [20, 60, 100, 140, 180, 220];

        return importantCycleNumbers.Contains(currentCycle) ? X * currentCycle : 0;
    }

    static void Draw(char[,] screen, int currentCycle, int X)
    {
        var posX = (currentCycle - 1) % 40;
        var posY = (currentCycle - 1) / 40;

        screen[posX, posY] = (X - 1 <= posX && X + 1 >= posX) ? '#' : '.';
    }

    static IEnumerable<Command> LoadFile(string fileName)
    {
        var lines = File.ReadAllLines(fileName);

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
