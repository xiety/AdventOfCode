using Advent.Common;

namespace A2022.Problem14;

public class Solver : IProblemSolver<int>
{
    public int RunA(string filename)
    {
        var items = Loader.Load(filename);
        var (width, height) = CalcSize(items);
        var sandbox = new Sandbox(width, height);

        return Simulate(sandbox, items);
    }

    public int RunB(string filename)
    {
        var items = Loader.Load(filename);

        var (width, height) = CalcSize(items);

        height += 2;
        items.Add([new(0, height - 1), new(width - 1, height - 1)]);

        var sandbox = new Sandbox(width, height);

        return Simulate(sandbox, items);
    }

    private static int Simulate(Sandbox sandbox, List<Pos[]> items)
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
                    return sands; //early return

                if (results.Moved == 0)
                    break;
            }
            while (true);

            sands++;
        }
        while (true);

        return sands;
    }

    private static (int, int) CalcSize(List<Pos[]> items)
    {
        var width = items.SelectMany(a => a).Max(a => a.X) + 1000;
        var height = items.SelectMany(a => a).Max(a => a.Y) + 1;
        return (width, height);
    }
}
