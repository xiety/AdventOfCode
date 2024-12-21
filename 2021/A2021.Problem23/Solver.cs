using System.Diagnostics;

using Advent.Common;

namespace A2021.Problem23;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        throw new NotImplementedException();

        var units = LoadFile(filename);

        return Recurse(0, 0, Int32.MaxValue, units, -1);
    }

    public long RunB(string filename)
    {
        throw new NotImplementedException();
    }

    readonly Dictionary<string, int> history = [];

    int Recurse(int level, int cost, int parentBestCost, Unit[] units, int lastUnitIndex)
    {
        var key = CreateKey(units);

        if (history.TryGetValue(key, out var result) && result <= cost)
            return result;

        if (units.All(a => a.Type == a.Node.Type))
        {
            Console.WriteLine($"{DateTime.Now} {cost}");
            return cost;
        }

        var bestFinishCost = parentBestCost;

        var situation = CalcSituation(units);

        for (var unitIndex = 0; unitIndex < units.Length; ++unitIndex)
        {
            if (unitIndex == lastUnitIndex)
                continue;

            var unit = units[unitIndex];

            var possibleMoves = GetPossibleMoves(unit, situation).ToArray();

            foreach (var possibleMove in possibleMoves)
            {
                var newCost = cost + costs[unit.Type] * possibleMove.Weight;

                if (newCost < bestFinishCost)
                {
                    var backup = unit.Node;

                    units[unitIndex] = units[unitIndex] with { Node = possibleMove.Target };

                    var finishCost = Recurse(level + 1, newCost, bestFinishCost, units, unitIndex);

                    if (finishCost < bestFinishCost)
                        bestFinishCost = finishCost;

                    units[unitIndex] = units[unitIndex] with { Node = backup };
                }
            }
        }

        history[key] = bestFinishCost;

        return bestFinishCost;
    }

    static string CreateKey(Unit[] units)
        => String.Join("|", units.Select(a => $"{a.Type}-{a.Node.Name}"));

    static Situation CalcSituation(Unit[] units)
    {
        var foreigners = new List<NodeType>();

        foreach (var unit in units)
        {
            if (unit.Type != unit.Node.Type && unit.Node.IsRoom)
                foreigners.Add(unit.Node.Type);
        }

        var occupied = units.ToArray(a => a.Node);

        return new([..foreigners], occupied);
    }

    static IEnumerable<GraphConnection> GetPossibleMoves(Unit unit, Situation situation)
    {
        var destinations = GetAllDestintations(unit, situation);

        foreach (var destination in destinations)
        {
            // destination is own room
            if (destination.Target.Type == unit.Type)
            {
                // source is own room
                if (unit.Node.Type == unit.Type)
                {
                }
                // source is foreign room or hall
                else
                {
                    var noForeigners = !situation.Foreigners.Contains(unit.Type);

                    if (noForeigners)
                    {
                        var isVeryBottom = !destinations
                            .Any(a => a.Target.Type == destination.Target.Type
                                   && a.Target.Coordinates.Y > destination.Target.Coordinates.Y);

                        if (isVeryBottom)
                            yield return destination;
                    }
                }
            }
            // destination is hall
            else if (destination.Target.Type == NodeType.Hall)
            {
                // source is room
                if (unit.Node.Type is NodeType.A or NodeType.B or NodeType.C or NodeType.D)
                {
                    // foreign room
                    if (unit.Node.Type != unit.Type)
                    {
                        yield return destination;
                    }
                    // own room
                    else
                    {
                        var noForeigners = !situation.Foreigners.Contains(unit.Type);

                        if (!noForeigners)
                            yield return destination;
                    }
                }
            }
        }
    }

    static GraphConnection[] GetAllDestintations(Unit unit, Situation situation)
    {
        var starts = new List<GraphConnection>() { new() { Target = unit.Node, Weight = 0 } };
        var history = new List<GraphNode>();
        var endings = new List<GraphConnection>();
        var newStarts = new List<GraphConnection>();

        do
        {
            foreach (var start in starts)
            {
                foreach (var connection in start.Target.Connections)
                {
                    if (situation.Occupied.Contains(connection.Target))
                        continue;

                    var newWeight = start.Weight + connection.Weight;

                    var newConnection = new GraphConnection() { Target = connection.Target, Weight = newWeight };

                    if (!history.Contains(newConnection.Target))
                    {
                        newStarts.Add(newConnection);
                        endings.Add(newConnection);
                        history.Add(newConnection.Target);
                    }
                }
            }

            (starts, newStarts) = (newStarts, starts);
            newStarts.Clear();
        }
        while (starts.Count > 0);

        return endings.ToArray();
    }

    readonly Dictionary<NodeType, int> costs = new()
    {
        [NodeType.A] = 1,
        [NodeType.B] = 10,
        [NodeType.C] = 100,
        [NodeType.D] = 1000,
    };

    static Unit[] LoadFile(string filename)
    {
        var hall1 = new GraphNode { Name = "Hall1", Type = NodeType.Hall, Coordinates = new(0, 0) };
        var hall2 = new GraphNode { Name = "Hall2", Type = NodeType.Hall, Coordinates = new(1, 0) };
        var hall3 = new GraphNode { Name = "Hall3", Type = NodeType.Hall, Coordinates = new(3, 0) };
        var hall4 = new GraphNode { Name = "Hall4", Type = NodeType.Hall, Coordinates = new(5, 0) };
        var hall5 = new GraphNode { Name = "Hall5", Type = NodeType.Hall, Coordinates = new(7, 0) };
        var hall6 = new GraphNode { Name = "Hall6", Type = NodeType.Hall, Coordinates = new(9, 0) };
        var hall7 = new GraphNode { Name = "Hall7", Type = NodeType.Hall, Coordinates = new(11, 0) };

        var roomA1 = new GraphNode { Name = "RoomA1", Type = NodeType.A, Coordinates = new(2, 1) };
        var roomA2 = new GraphNode { Name = "RoomA2", Type = NodeType.A, Coordinates = new(2, 2) };

        var roomB1 = new GraphNode { Name = "RoomB1", Type = NodeType.B, Coordinates = new(4, 1) };
        var roomB2 = new GraphNode { Name = "RoomB2", Type = NodeType.B, Coordinates = new(4, 2) };

        var roomC1 = new GraphNode { Name = "RoomC1", Type = NodeType.C, Coordinates = new(6, 1) };
        var roomC2 = new GraphNode { Name = "RoomC2", Type = NodeType.C, Coordinates = new(6, 2) };

        var roomD1 = new GraphNode { Name = "RoomD1", Type = NodeType.D, Coordinates = new(8, 1) };
        var roomD2 = new GraphNode { Name = "RoomD2", Type = NodeType.D, Coordinates = new(8, 2) };

        hall1.ConnectTo(hall2, 1);
        hall2.ConnectTo(hall3, 2);
        hall3.ConnectTo(hall4, 2);
        hall4.ConnectTo(hall5, 2);
        hall5.ConnectTo(hall6, 2);
        hall6.ConnectTo(hall7, 1);

        hall2.ConnectTo(roomA1, 2);
        hall3.ConnectTo(roomA1, 2);

        hall3.ConnectTo(roomB1, 2);
        hall4.ConnectTo(roomB1, 2);

        hall4.ConnectTo(roomC1, 2);
        hall5.ConnectTo(roomC1, 2);

        hall5.ConnectTo(roomD1, 2);
        hall6.ConnectTo(roomD1, 2);

        roomA1.ConnectTo(roomA2, 1);
        roomB1.ConnectTo(roomB2, 1);
        roomC1.ConnectTo(roomC2, 1);
        roomD1.ConnectTo(roomD2, 1);

        var lines = File.ReadAllLines(filename);
        var units = new List<Unit>();

        GraphNode[] rooms = [roomA1, roomA2, roomB1, roomB2, roomC1, roomC2, roomD1, roomD2];

        for (var y = 0; y < lines.Length; ++y)
        {
            for (var x = 0; x < lines[0].Length; ++x)
            {
                var room = rooms.FirstOrDefault(a => a.Coordinates.X == x - 1 && a.Coordinates.Y == y - 1);

                if (room is not null)
                {
                    var c = lines[y][x];

                    switch (c)
                    {
                        case 'A':
                            units.Add(new(NodeType.A, room));
                            break;
                        case 'B':
                            units.Add(new(NodeType.B, room));
                            break;
                        case 'C':
                            units.Add(new(NodeType.C, room));
                            break;
                        case 'D':
                            units.Add(new(NodeType.D, room));
                            break;
                    }
                }
            }
        }

        return units.OrderBy(a => a.Type).ToArray();
    }
}

[DebuggerDisplay("Name={Name} Type={Type}")]
public class GraphNode
{
    public required string Name { get; init; }
    public required NodeType Type { get; init; }
    public required Pos Coordinates { get; init; }

    public List<GraphConnection> Connections { get; } = [];

    public bool IsRoom => Type is NodeType.A or NodeType.B or NodeType.C or NodeType.D;

    public void ConnectTo(GraphNode target, int weight)
    {
        Connections.Add(new() { Target = target, Weight = weight });
        target.Connections.Add(new() { Target = this, Weight = weight });
    }
}

[DebuggerDisplay("Target={Target} Weight={Weight}")]
public class GraphConnection
{
    public required int Weight { get; init; }
    public required GraphNode Target { get; init; }
}

public enum NodeType { Empty, Hall, Skip, Wall, A, B, C, D, }

[DebuggerDisplay("Type={Type} Node={Node}")]
public record Unit(NodeType Type, GraphNode Node);

public record Situation(NodeType[] Foreigners, GraphNode[] Occupied);
