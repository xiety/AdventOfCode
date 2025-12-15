using System.Text.RegularExpressions;

using Advent.Common;

namespace A2021.Problem05;

public static class Solver
{
    [GeneratedTest<int>(5, 6856)]
    public static int RunA(string[] lines)
        => Run(lines, false);

    [GeneratedTest<int>(12, 20666)]
    public static int RunB(string[] lines)
        => Run(lines, true);

    static int Run(string[] lines, bool diagonal)
    {
        var items = CompiledRegs.FromLinesRegex(lines);

        var (width, height) = FindSize(items);

        var array = new int[width, height];

        foreach (var item in items)
        {
            if (item.FromX == item.ToX)
                DrawVertical(array, item);
            else if (item.FromY == item.ToY)
                DrawHorizontal(array, item);
            else if (diagonal)
                DrawDiagonal(array, item);
        }

        return array.Cast<int>().Count(a => a > 1);
    }

    static void DrawDiagonal(int[,] array, Item item)
    {
        var signX = item.FromX < item.ToX ? 1 : -1;
        var signY = item.FromY < item.ToY ? 1 : -1;

        var max = Math.Max(item.FromX, item.ToX) - Math.Min(item.FromX, item.ToX) + 1;

        for (var i = 0; i < max; ++i)
            array[item.FromX + (i * signX), item.FromY + (i * signY)]++;
    }

    static void DrawHorizontal(int[,] array, Item item)
    {
        var (from, to) = item.FromX < item.ToX ? (item.FromX, item.ToX) : (item.ToX, item.FromX);

        for (var x = from; x <= to; ++x)
            array[x, item.FromY]++;
    }

    static void DrawVertical(int[,] array, Item item)
    {
        var (from, to) = item.FromY < item.ToY ? (item.FromY, item.ToY) : (item.ToY, item.FromY);

        for (var y = from; y <= to; ++y)
            array[item.FromX, y]++;
    }

    static (int, int) FindSize(Item[] items)
    {
        var maxX = Math.Max(items.Max(a => a.FromX), items.Max(a => a.ToX));
        var maxY = Math.Max(items.Max(a => a.FromY), items.Max(a => a.ToY));

        return (maxX + 1, maxY + 1);
    }
}

readonly record struct Item(int FromX, int FromY, int ToX, int ToY);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Item.FromX)}>\d+),(?<{nameof(Item.FromY)}>\d+) -> (?<{nameof(Item.ToX)}>\d+),(?<{nameof(Item.ToY)}>\d+)$")]
    [MapTo<Item>]
    public static partial Regex Regex();
}
