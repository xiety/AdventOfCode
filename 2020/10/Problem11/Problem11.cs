using Advent.Common;

namespace A2020.Problem11;

public static class Solver
{
    [GeneratedTest<int>(37, 2289)]
    public static int RunA(string[] lines)
        => Run(lines, 4, CountOccupiedA);

    [GeneratedTest<int>(26, 2059)]
    public static int RunB(string[] lines)
        => Run(lines, 5, CountOccupiedB);

    static int Run(string[] lines, int max, CountFunc countFunc)
    {
        var map = LoadData(lines);
        Fors.LoopWhile(() => Mutate(map, max, countFunc));
        return map.EnumeratePositionsOf(Seat.Occupied).Count();
    }

    static bool Mutate(Seat[,] map, int max, CountFunc countFunc)
    {
        var query1 = Query(map, Seat.Occupied, Seat.Empty, countFunc, a => a >= max);
        var query2 = Query(map, Seat.Empty, Seat.Occupied, countFunc, a => a == 0);

        var changes = (query1 + query2).ToArray();

        foreach (var (pos, newItem) in changes)
            map.Set(pos, newItem);

        return changes.Any();
    }

    static IEnumerable<(Pos pos, Seat Empty)> Query(Seat[,] map, Seat fromSeat, Seat toSeat, CountFunc countFunc, Func<int, bool> check)
        => from pos in map.EnumeratePositionsOf(fromSeat)
           let occupied = countFunc(map, pos)
           where check(occupied)
           select (pos, toSeat);

    static int CountOccupiedA(Seat[,] map, Pos pos)
        => map.EnumerateDelted(pos).Count(a => a.Item == Seat.Occupied);

    static int CountOccupiedB(Seat[,] map, Pos pos)
        => map.Deltas(pos).Count(a => CheckB(map, pos, a));

    static bool CheckB(Seat[,] map, Pos start, Pos dir)
        => start.EnumerateRay(dir)
            .TakeWhile(map.IsInBounds)
            .Select(map.Get)
            .FirstOrDefault(a => a != Seat.Floor) == Seat.Occupied;

    static Seat[,] LoadData(string[] lines)
        => MapData.ParseMap(lines, a => a switch { 'L' => Seat.Empty, '.' => Seat.Floor });

    delegate int CountFunc(Seat[,] map, Pos pos);
}

enum Seat { Floor, Empty, Occupied }
