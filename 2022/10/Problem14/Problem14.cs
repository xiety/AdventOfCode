using Advent.Common;

namespace A2022.Problem14;

public static class Solver
{
    [GeneratedTest<int>(24, 768)]
    public static int RunA(string[] lines)
    {
        var items = LoadData(lines);
        var (width, height) = CalcSize(items);
        var sandbox = new Sandbox(width, height);

        return Simulate(sandbox, items);
    }

    [GeneratedTest<int>(93, 26686)]
    public static int RunB(string[] lines)
    {
        var items = LoadData(lines).ToList();

        var (width, height) = CalcSize(items);

        height += 2;
        items.Add([new(0, height - 1), new(width - 1, height - 1)]);

        var sandbox = new Sandbox(width, height);

        return Simulate(sandbox, items);
    }

    static int Simulate(Sandbox sandbox, ICollection<Pos[]> items)
    {
        sandbox.AddWalls(items);

        var sandEmitterPos = new Pos(500, 0);

        var sands = 0;

        do
        {
            var emitted = sandbox.Emit(sandEmitterPos, UnitType.Sand);

            if (!emitted)
                break;

            do
            {
                var results = sandbox.RunStep();

                if (results.OutOfBounds > 0)
                    return sands;

                if (results.Moved == 0)
                    break;
            }
            while (true);

            sands++;
        }
        while (true);

        return sands;
    }

    static (int, int) CalcSize(ICollection<Pos[]> items)
    {
        var width = items.SelectMany(a => a).Max(a => a.X) + 1000;
        var height = items.SelectMany(a => a).Max(a => a.Y) + 1;
        return (width, height);
    }

    static Pos[][] LoadData(string[] lines)
        => lines.ToArray(ParseLine);

    static Pos[] ParseLine(string line)
        => line.Split(" -> ")
               .ToArray(Pos.Parse);
}
