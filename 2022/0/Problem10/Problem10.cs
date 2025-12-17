using System.Text.RegularExpressions;

namespace A2022.Problem10;

public static class Solver
{
    [GeneratedTest<int>(13140, 13820)]
    public static int RunA(string[] lines)
    {
        var commands = LoadData(lines);

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
                    foreach (var _ in 2)
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

    [GeneratedTest<string>(ResultData.Result09A, ResultData.Result09B)]
    public static string RunB(string[] lines)
    {
        var commands = LoadData(lines);

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

        return screen.ToDump(Environment.NewLine, "", a => a == 0 ? " " : $"{a}")
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

    static IEnumerable<Command> LoadData(string[] lines)
    {
        foreach (var item in lines)
        {
            if (item is "noop")
            {
                yield return new CommandNoop();
            }
            else
            {
                var addx = CompiledRegs.MapToRegexAddx(item);

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
    [MapTo<ParsedAddx>]
    public static partial Regex RegexAddx();
}

static class ResultData
{
    public const string Result09A = """
        ##..##..##..##..##..##..##..##..##..##..
        ###...###...###...###...###...###...###.
        ####....####....####....####....####....
        #####.....#####.....#####.....#####.....
        ######......######......######......####
        #######.......#######.......#######.....
        """;

    public const string Result09B = """
        ####.#..#..##..###..#..#..##..###..#..#.
        ...#.#.#..#..#.#..#.#.#..#..#.#..#.#.#..
        ..#..##...#....#..#.##...#....#..#.##...
        .#...#.#..#.##.###..#.#..#.##.###..#.#..
        #....#.#..#..#.#.#..#.#..#..#.#.#..#.#..
        ####.#..#..###.#..#.#..#..###.#..#.#..#.
        """;
}
