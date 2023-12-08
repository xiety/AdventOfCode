using Advent.Common;

namespace A2021.Problem11;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
        => Calc(filename, (data, step) => step == 100).Item2;

    public long RunB(string filename)
        => Calc(filename, (data, step) => data.Enumerate().All(a => a.item == 0)).Item1;

    public (long, long) Calc(string filename, Func<int[,], long, bool> exit)
    {
        var data = MapData.ParseMap(File.ReadAllLines(filename));

        var counter = 0;

        var highlighteds = new List<Pos>();

        var step = 1;

        do
        {
            foreach (var pos in data.EnumeratePositions())
                data[pos.X, pos.Y]++;

            foreach (var pos in data.EnumeratePositions())
            {
                if (data.Get(pos) == 10)
                {
                    data.Set(pos, 0);
                    highlighteds.Add(pos);
                }
            }

            do
            {
                counter += highlighteds.Count;

                var copy = highlighteds.ToArray();
                highlighteds.Clear();

                foreach (var highlighted in copy)
                {
                    foreach (var offset in EnumerableExtensions
                        .Range2d(3, 3)
                        .Select(a => highlighted + a + new Pos(-1, -1))
                        .Where(data.IsInBounds))
                    {
                        ref var r = ref data.GetRef(offset);

                        if (r != 0)
                            r++;

                        if (r == 10)
                        {
                            r = 0;
                            highlighteds.Add(offset);
                        }
                    }
                }
            }
            while (highlighteds.Count > 0);

            if (exit(data, step))
                break;

            step++;
        }
        while (true);

        return (step, counter);
    }
}
