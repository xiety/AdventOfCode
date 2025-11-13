using System.Collections;

using Advent.Common;

namespace A2021.Problem20;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
        => Run(filename, 2);

    public long RunB(string filename)
        => Run(filename, 50);

    static long Run(string filename, int totalSteps)
    {
        var (palette, map) = LoadFile(filename);

        var width = map.Width;
        var height = map.Height;

        var space = new bool[width + totalSteps * 2 + 2, height + totalSteps * 2 + 2];

        var offsetX = totalSteps + 1;
        var offsetY = totalSteps + 1;

        for (var y = 0; y < height; ++y)
            for (var x = 0; x < width; ++x)
                space[x + offsetX, y + offsetY] = map[x, y];

        var tempArray = new int[1];
        var ba = new BitArray(9);

        var output = new bool[space.Width, space.Height];

        for (var step = 0; step < totalSteps; ++step)
        {
            for (var y = 1; y < space.Height - 1; ++y)
            {
                for (var x = 1; x < space.Width - 1; ++x)
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

                    var pn = palette[n];

                    output[x, y] = pn;
                }
            }

            (space, output) = (output, space);
            Array.Clear(output);
        }

        var result = space.EnumeratePositionsOf(true).Count();

        return result;
    }

    static (bool[], bool[,]) LoadFile(string filename)
    {
        var chunks = File.ReadAllLines(filename).SplitBy(String.Empty).ToArray();
        var palette = chunks[0].First().ToArray(a => a == '#');
        var map = MapData.ParseMap([.. chunks[1]], a => a == '#');
        return (palette, map);
    }
}
