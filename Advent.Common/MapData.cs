namespace Advent.Common;

public static class MapData
{
    public static int[,] ParseMap(string[] lines)
        => ParseMap(lines, a => int.Parse($"{a}"));

    public static T[,] ParseMap<T>(string[] lines, Func<char, T> parse)
    {
        var height = lines.Length;
        var width = lines[0].Length;
        var data = new T[width, height];

        for (var y = 0; y < height; ++y)
            for (var x = 0; x < width; ++x)
                data[x, y] = parse(lines[y][x]);

        return data;
    }

    public static Pos FindPos(string[] lines, char c)
    {
        var height = lines.Length;
        var width = lines[0].Length;

        for (var y = 0; y < height; ++y)
            for (var x = 0; x < width; ++x)
                if (lines[y][x] == c)
                    return new(x, y);

        throw new("Not found");
    }
}
