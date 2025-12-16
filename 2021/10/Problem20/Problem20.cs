using System.Collections;

namespace A2021.Problem20;

public static class Solver
{
    [GeneratedTest<long>(35, 5479)]
    public static long RunA(string[] lines)
        => Run(lines, 2);

    [GeneratedTest<long>(3351, 19012)]
    public static long RunB(string[] lines)
        => Run(lines, 50);

    static long Run(string[] lines, int totalSteps)
    {
        var (palette, map) = LoadData(lines);

        var width = map.Width;
        var height = map.Height;

        var space = new bool[width + totalSteps * 2 + 2, height + totalSteps * 2 + 2];

        var offsetX = totalSteps + 1;
        var offsetY = totalSteps + 1;

        foreach (var y in height)
            foreach (var x in width)
                space[x + offsetX, y + offsetY] = map[x, y];

        var tempArray = new int[1];
        var ba = new BitArray(9);

        var output = new bool[space.Width, space.Height];

        foreach (var step in totalSteps)
        {
            foreach (var y in 1..(space.Height - 1))
            {
                foreach (var x in 1..(space.Width - 1))
                {
                    var bit = 8;

                    for (var dy = -1; dy <= 1; ++dy)
                    {
                        for (var dx = -1; dx <= 1; ++dx)
                        {
                            var (ox, oy) = (x + dx, y + dy);

                            var isBorder = ox == 0
                                        || oy == 0
                                        || ox == space.Width - 1
                                        || oy == space.Height - 1;

                            if (isBorder && palette[0] && !palette[^1])
                                ba[bit] = step % 2 == 1;
                            else
                                ba[bit] = space[ox, oy];

                            bit--;
                        }
                    }

                    ba.CopyTo(tempArray, 0);
                    var n = tempArray[0];

                    output[x, y] = palette[n];
                }
            }

            (space, output) = (output, space);
            Array.Clear(output);
        }

        return space.EnumeratePositionsOf(true).Count();
    }

    static (bool[], bool[,]) LoadData(string[] lines)
    {
        var chunks = lines.SplitBy(String.Empty).ToArray();
        var palette = chunks[0][0].ToArray(a => a == '#');
        var map = MapData.ParseMap([.. chunks[1]], a => a == '#');
        return (palette, map);
    }
}
