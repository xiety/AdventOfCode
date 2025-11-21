using System.Text.RegularExpressions;
using static System.Linq.Enumerable;

using Advent.Common;

namespace A2020.Problem09;

public class Solver : ISolver<long>
{
    public long RunA(string[] lines, bool isSample)
    {
        var items = LoadItems(lines);
        var size = isSample ? 5 : 25;
        return FindBadNumber(items, size).Item;
    }

    public long RunB(string[] lines, bool isSample)
    {
        var items = LoadItems(lines);
        var size = isSample ? 5 : 25;
        var bad = FindBadNumber(items, size);

        var part = Range(0, bad.Index - 1)
            .SelectMany(start => Range(2, bad.Index - start - 1),
                (start, len) => ArraySegment.From(items, start, len))
            .First(part => part.Sum() == bad.Item);

        return part.Min() + part.Max();
    }

    static (int Index, long Item) FindBadNumber(long[] items, int size)
        => items
            .Index()
            .Skip(size)
            .First(a => !Check(items.AsSpan((a.Index - size)..a.Index), a.Item));

    static bool Check(ReadOnlySpan<long> items, long number)
    {
        for (var i = 0; i < items.Length - 1; ++i)
            if (items[(i + 1)..].Contains(number - items[i]))
                return true;

        return false;
    }

    static long[] LoadItems(string[] lines)
        => lines.Select(long.Parse).ToArray();
}
