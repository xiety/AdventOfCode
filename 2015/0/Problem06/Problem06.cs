using System.Text.RegularExpressions;

namespace A2015.Problem06;

public static class Solver
{
    [GeneratedTest<int>(1000 * 1000 - 1000 - 4, 377891)]
    public static int RunA(string[] lines) =>
        Solve<bool>(lines, CalcA, ScoreA);

    [GeneratedTest<int>(2000000 + 1, 14110788)]
    public static int RunB(string[] lines) =>
        Solve<int>(lines, CalcB, ScoreB);

    static int Solve<T>(string[] lines, Func<Mode, T, T> step, Func<T, int> result)
    {
        var grid = new T[1000, 1000];

        foreach (var item in LoadItems(lines))
            for (var x = item.Rect.P1.X; x <= item.Rect.P2.X; ++x)
                for (var y = item.Rect.P1.Y; y <= item.Rect.P2.Y; ++y)
                    grid[x, y] = step(item.Mode, grid[x, y]);

        return grid
            .EnumerateItems()
            .Sum(result);
    }

    [GeneratedTest<int>(1000 * 1000 - 1000 - 4, 377891)]
    public static int RunALinq(string[] lines) =>
        SolveLinq(lines, false, CalcA, ScoreA);

    [GeneratedTest<int>(2000000 + 1, 14110788)]
    public static int RunBLinq(string[] lines) =>
        SolveLinq(lines, 0, CalcB, ScoreB);

    static int SolveLinq<T>(string[] lines, T initialValue, Func<Mode, T, T> apply, Func<T, int> score) =>
        LoadItems(lines)
            .SelectMany(a => AllPositions(a.Rect).Select(p => (p, a.Mode)))
            .GroupBy(x => x.p)
            .Sum(g => score(g.Aggregate(initialValue, (s, x) => apply(x.Mode, s))));

    static IEnumerable<Pos> AllPositions(Rect r) =>
        from x in Enumerable.Range(r.P1.X, r.P2.X - r.P1.X + 1)
        from y in Enumerable.Range(r.P1.Y, r.P2.Y - r.P1.Y + 1)
        select new Pos(x, y);

    [GeneratedTest<int>(1000 * 1000 - 1000 - 4, 377891)]
    public static int RunACompressed(string[] lines) =>
        SolveCompressed(LoadItems(lines), false, CalcA, (a, r) => a ? (int)r.Area : 0);

    [GeneratedTest<int>(2000000 + 1, 14110788)]
    public static int RunBCompressed(string[] lines) =>
        SolveCompressed(LoadItems(lines), 0, CalcB, (a, r) => a * (int)r.Area);

    static int SolveCompressed<T>(Item[] items, T initialValue, Func<Mode, T, T> apply, Func<T, Rect, int> score) =>
        GetRects(items).Sum(rect => CalcCompressed(items, initialValue, apply, score, rect));

    static int CalcCompressed<T>(Item[] items, T initialValue, Func<Mode, T, T> apply, Func<T, Rect, int> score, Rect rect)
    {
        var result = items
            .Where(a => a.Rect.InsideMe(rect))
            .Aggregate(initialValue, (acc, item) => apply(item.Mode, acc));

        return score(result, rect);
    }

    static IEnumerable<Rect> GetRects(Item[] items)
    {
        var xs = (items.Select(a => a.Rect.P1.X) + items.Select(a => a.Rect.P2.X)).Distinct().Order().ToArray();
        var ys = (items.Select(a => a.Rect.P1.Y) + items.Select(a => a.Rect.P2.Y)).Distinct().Order().ToArray();

        return Fors.For((0, xs.Length), (0, ys.Length))
            .SelectMany(a => Parts(xs, ys, a[0], a[1]));
    }

    static IEnumerable<Rect> Parts(int[] xs, int[] ys, int xi, int yi)
    {
        var (x, y) = (xs[xi], ys[yi]);

        yield return new(new(x, y), new(x, y));

        if (xi < xs.Length - 1 && xs[xi + 1] - x >= 2)
            yield return new(new(x + 1, y), new(xs[xi + 1] - 1, y));

        if (yi < ys.Length - 1 && ys[yi + 1] - y >= 2)
            yield return new(new(x, y + 1), new(x, ys[yi + 1] - 1));

        if (xi < xs.Length - 1 && yi < ys.Length - 1 && xs[xi + 1] - x >= 2 && ys[yi + 1] - y >= 2)
            yield return new(new(x + 1, y + 1), new(xs[xi + 1] - 1, ys[yi + 1] - 1));
    }

    static bool CalcA(Mode mode, bool state) =>
        mode switch
        {
            Mode.TurnOn => true,
            Mode.TurnOff => false,
            Mode.Toggle => !state,
        };

    static int CalcB(Mode mode, int state) =>
        mode switch
        {
            Mode.TurnOn => state + 1,
            Mode.TurnOff => state > 0 ? state - 1 : 0,
            Mode.Toggle => state + 2,
        };

    static int ScoreA(bool value) =>
        value ? 1 : 0;

    static int ScoreB(int value) =>
        value;

    static Item[] LoadItems(string[] lines) =>
        CompiledRegs.FromLinesRegex(lines)
            .ToArray(a => new Item(a.Mode, new(new(a.X1, a.Y1), new(a.X2, a.Y2))));
}

record ItemRaw([RegexParser<ParseMode, Mode>] Mode Mode, int X1, int Y1, int X2, int Y2);
record Item(Mode Mode, Rect Rect);

enum Mode
{
    TurnOn,
    TurnOff,
    Toggle,
}

class ParseMode : IRegexParser<Mode>
{
    public Mode Parse(string text) =>
        text switch
        {
            "turn on" => Mode.TurnOn,
            "turn off" => Mode.TurnOff,
            "toggle" => Mode.Toggle,
        };
}

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(ItemRaw.Mode)}>turn on|turn off|toggle) (?<{nameof(ItemRaw.X1)}>\d+),(?<{nameof(ItemRaw.Y1)}>\d+) through (?<{nameof(ItemRaw.X2)}>\d+),(?<{nameof(ItemRaw.Y2)}>\d+)$")]
    [MapTo<ItemRaw>]
    public static partial Regex Regex();
}
