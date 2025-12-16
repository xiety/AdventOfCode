namespace A2021.Problem11;

public static class Solver
{
    [GeneratedTest<long>(1656, 1613)]
    public static long RunA(string[] lines)
        => Calc(lines, (_, step) => step == 100).Item2;

    [GeneratedTest<long>(195, 510)]
    public static long RunB(string[] lines)
        => Calc(lines, (data, _) => data.Enumerate().All(a => a.Item == 0)).Item1;

    static (long, long) Calc(string[] lines, Func<int[,], long, bool> exit)
    {
        var data = MapData.ParseMap(lines);

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
                    foreach (var offset in data.Delted(highlighted))
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
