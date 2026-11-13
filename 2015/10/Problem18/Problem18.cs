namespace A2015.Problem18;

public static class Solver
{
    [GeneratedTest<int>(4, 814)]
    public static int RunA(string[] lines, bool isSample)
        => Calc(lines, isSample ? 4 : 100);

    [GeneratedTest<int>(17, 924)]
    public static int RunB(string[] lines, bool isSample)
        => Calc(lines, isSample ? 5 : 100, Glitch);

    static int Calc(string[] lines, int total, Action<bool[,]>? work = null)
    {
        var map = LoadItems(lines);

        work?.Invoke(map);

        foreach (var step in total)
        {
            MakeStep(map);
            work?.Invoke(map);
        }

        return map.EnumerateItems().Count(a => a);
    }

    static void MakeStep(bool[,] map)
    {
        var copy = map.CloneArray();

        foreach (var pos in map.EnumeratePositions())
        {
            var c = map.Get(pos);
            var n = copy.EnumerateDeltedItems(pos).Count(a => a);
            map.Set(pos, (c, n) is (_, 3) or (true, 2));
        }
    }

    static void Glitch(bool[,] map)
    {
        map.Set(0, 0, true);
        map.Set(0, ^1, true);
        map.Set(^1, 0, true);
        map.Set(^1, ^1, true);
    }

    static bool[,] LoadItems(string[] lines)
        => MapData.ParseMap(lines, a => a == '#');
}
