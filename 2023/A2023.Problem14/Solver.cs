using Advent.Common;

namespace A2023.Problem14;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var map = MapData.ParseMap(File.ReadAllLines(filename), c => c switch
        { '.' => NodeType.Empty, '#' => NodeType.Wall, 'O' => NodeType.Ball, });

        North(map);

        var result = map.EnumeratePositionsOf(NodeType.Ball)
                        .Select(a => map.GetWidth() - a.Y)
                        .Sum();

        return result;
    }

    private static void North(NodeType[,] map)
    {
        var south = new int[map.GetWidth()];

        for (var y = 0; y < map.GetHeight(); ++y)
        {
            for (var x = 0; x < map.GetWidth(); ++x)
            {
                var c = map[x, y];

                if (c == NodeType.Wall)
                {
                    south[x] = y + 1;
                }
                else if (c == NodeType.Ball)
                {
                    map[x, y] = NodeType.Empty;
                    map[x, south[x]] = NodeType.Ball;
                    south[x]++;
                }
            }
        }
    }
}

public enum NodeType
{
    Empty,
    Wall,
    Ball,
}
