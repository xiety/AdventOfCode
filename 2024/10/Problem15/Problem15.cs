using Advent.Common;

namespace A2024.Problem15;

public static class Solver
{
    [GeneratedTest<long>(10092, 1471826)]
    public static long RunA(string[] lines)
    {
        var (map, moves) = LoadData(lines);
        return Simulate(map, moves);
    }

    [GeneratedTest<long>(9021, 1457703)]
    public static long RunB(string[] lines)
    {
        var (map, moves) = LoadData(lines);
        map = Enlarge(map);
        return Simulate(map, moves);
    }

    static long Simulate(NodeType[,] map, Pos[] moves)
    {
        var pos = FindStartPos(map);

        foreach (var move in moves)
        {
            var canMove = MoveNode(map, pos, move, false);

            if (canMove)
            {
                MoveNode(map, pos, move, true);
                pos += move;
            }
        }

        return CalculateCount(map);
    }

    static bool MoveNode(NodeType[,] map, Pos pos, Pos move, bool makeMove)
    {
        var newPos = pos + move;
        var target = map.Get(newPos);

        var canMove = target switch
        {
            NodeType.Box => MoveNode(map, newPos, move, makeMove),
            NodeType.BoxLeft => MoveNode(map, newPos, move, makeMove)
                && (move.X != 0 || MoveNode(map, newPos + new Pos(1, 0), move, makeMove)),
            NodeType.BoxRight => MoveNode(map, newPos, move, makeMove)
                && (move.X != 0 || MoveNode(map, newPos + new Pos(-1, 0), move, makeMove)),
            NodeType.None => true,
            _ => false,
        };

        if (canMove && makeMove)
        {
            var node = map.Get(pos);
            map.Set(pos, NodeType.None);
            map.Set(newPos, node);
        }

        return canMove;
    }

    static int CalculateCount(NodeType[,] map)
        => map.EnumeratePositionsOf(NodeType.Box, NodeType.BoxLeft).Sum(a => a.Y * 100 + a.X);

    static Pos FindStartPos(NodeType[,] map)
        => map.EnumeratePositionsOf(NodeType.Unit).First();

    static NodeType[,] Enlarge(NodeType[,] map)
    {
        var newMap = new NodeType[map.Width * 2, map.Height];

        foreach (var (pos, item) in map.Enumerate())
        {
            var (left, right) = item switch
            {
                NodeType.Box => (NodeType.BoxLeft, NodeType.BoxRight),
                NodeType.Unit => (NodeType.Unit, NodeType.None),
                _ => (item, item),
            };

            newMap[pos.X * 2, pos.Y] = left;
            newMap[pos.X * 2 + 1, pos.Y] = right;
        }

        return newMap;
    }

    static (NodeType[,], Pos[]) LoadData(string[] lines)
    {
        var parts = lines.SplitBy(String.Empty).ToArray();

        var map = MapData.ParseMap(parts[0], a => a switch { '#' => NodeType.Wall, 'O' => NodeType.Box, '@' => NodeType.Unit, _ => NodeType.None });

        var moves = String.Concat(parts[1])
            .ToArray(c => c switch { '^' => new Pos(0, -1), '>' => new Pos(1, 0), '<' => new Pos(-1, 0), 'v' => new Pos(0, 1) });

        return (map, moves);
    }

    enum NodeType { None, Wall, Box, BoxLeft, BoxRight, Unit }
}
