namespace A2024.Problem04;

public static class Solver
{
    const string TargetA = "XMAS";
    const string TargetB = "MAS";

    [GeneratedTest<int>(18, 2397)]
    public static int RunA(string[] lines)
    {
        var map = MapData.ParseMap(lines, a => $"{a}");

        var query = from p in map.EnumeratePositions()
                    from d in map.Deltas()
                    let s = Get(map, p, d, TargetA.Length)
                    where s == TargetA
                    select 1;

        return query.Count();
    }

    [GeneratedTest<int>(9, 1824)]
    public static int RunB(string[] lines)
    {
        var map = MapData.ParseMap(lines, a => $"{a}");

        return map.EnumeratePositions()
            .Select(p => ArrayEx.DiagOffsets
                .Where(offset => map.IsInBounds(p + offset))
                .Select(offset => Get(map, p + offset, -offset, TargetB.Length))
                .Count(a => a == TargetB))
            .Count(a => a == 2);
    }

    static string Get(string[,] map, Pos p, Pos d, int len)
        => Enumerable.Range(0, len)
           .Aggregate((s: "", p), (acc, _) => (acc.s + map.GetOrDefault(acc.p, ""), acc.p + d)).s;
}
