using Advent.Common;

namespace A2019.Problem08;

public static class Solver
{
    [GeneratedTest<int>(4, 1206)]
    public static int RunA(string[] lines, bool isSample)
    {
        var width = isSample ? 2 : 25;
        var height = isSample ? 2 : 6;

        var layers = LoadData(lines, width, height);
        var layer = layers.MinBy(a => a.EnumeratePositionsOf(0).Count())!;
        return layer.EnumeratePositionsOf(1).Count() * layer.EnumeratePositionsOf(2).Count();
    }

    [GeneratedTest<string>(ResultData.Result08Sample, ResultData.Result08Input)]
    public static string RunB(string[] lines, bool isSample)
    {
        var width = isSample ? 2 : 25;
        var height = isSample ? 2 : 6;

        var layers = LoadData(lines, width, height).ToArray();

        var result = new int[width, height];

        foreach (var pos in result.EnumeratePositions())
            result.Set(pos, layers.SkipWhile(a => a.Get(pos) == 2).Select(a => a.Get(pos)).FirstOrDefault());

        return result.ToDump(Environment.NewLine, "", a => a == 1 ? "#" : ".").TrimEnd();
    }

    static int[][,] LoadData(string[] lines, int width, int height)
    {
        var text = lines[0];
        var numLayers = text.Length / (width * height);
        var ret = new int[numLayers][,];

        for (int z = 0, i = 0; z < numLayers; ++z)
        {
            ret[z] = new int[width, height];

            foreach (var y in height)
                for (var x = 0; x < width; ++x, ++i)
                    ret[z][x, y] = text[i] - '0';
        }

        return ret;
    }
}

public static class ResultData
{
    public const string Result08Sample = """
        .#
        #.
        """;

    public const string Result08Input = """
        ####...##.###...##..###..
        #.......#.#..#.#..#.#..#.
        ###.....#.#..#.#....#..#.
        #.......#.###..#.##.###..
        #....#..#.#.#..#..#.#....
        ####..##..#..#..###.#....
        """;
}
