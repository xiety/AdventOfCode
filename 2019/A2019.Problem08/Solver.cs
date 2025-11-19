using Advent.Common;

namespace A2019.Problem08;

public class Solver : ISolver<int, string>
{
    public int RunA(string[] lines, bool isSample)
    {
        var width = isSample ? 2 : 25;
        var height = isSample ? 2 : 6;

        var layers = LoadData(lines, width, height);
        var layer = layers.MinBy(a => a.EnumeratePositionsOf(0).Count())!;
        return layer.EnumeratePositionsOf(1).Count() * layer.EnumeratePositionsOf(2).Count();
    }

    public string RunB(string[] lines, bool isSample)
    {
        var width = isSample ? 2 : 25;
        var height = isSample ? 2 : 6;

        var layers = LoadData(lines, width, height).ToArray();

        var result = new int[width, height];

        foreach (var pos in result.EnumeratePositions())
            result.Set(pos, layers.SkipWhile(a => a.Get(pos) == 2).Select(a => a.Get(pos)).FirstOrDefault());

        return result.ToString(Environment.NewLine, "", a => a == 1 ? "#" : ".").TrimEnd();
    }

    static int[][,] LoadData(string[] lines, int width, int height)
    {
        var text = lines[0];
        var numLayers = text.Length / (width * height);
        var ret = new int[numLayers][,];

        for (int z = 0, i = 0; z < numLayers; ++z)
        {
            ret[z] = new int[width, height];

            for (var y = 0; y < height; ++y)
                for (var x = 0; x < width; ++x, ++i)
                    ret[z][x, y] = text[i] - '0';
        }

        return ret;
    }
}
