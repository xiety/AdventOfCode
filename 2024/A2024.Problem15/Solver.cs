using Advent.Common;

namespace A2024.Problem15;

public class Solver : ISolver<long>
{
    public long RunA(string[] lines, bool isSample)
    {
        var (map, moves) = LoadData(lines);
        return Simulate(map, moves);
    }

    public long RunB(string[] lines, bool isSample)
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
        var newMap = new NodeType[map.GetWidth() * 2, map.GetHeight()];

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
        var parts = lines.Split(String.Empty).ToArray();

        var map = MapData.ParseMap(parts[0], a => a switch { '#' => NodeType.Wall, 'O' => NodeType.Box, '@' => NodeType.Unit, _ => NodeType.None });

        var moves = String.Join(String.Empty, parts[1])
            .Select(c => c switch { '^' => new Pos(0, -1), '>' => new Pos(1, 0), '<' => new Pos(-1, 0), 'v' => new Pos(0, 1) })
            .ToArray();

        return (map, moves);
    }

    enum NodeType { None, Wall, Box, BoxLeft, BoxRight, Unit, }
}
