using Advent.Common;

namespace A2022.Problem13;

public static class Solver
{
    [GeneratedTest<int>(13, 6478)]
    public static int RunA(string[] lines)
    {
        var pairs = Loader.Load(lines);

        return pairs
            .Select((a, i) => new { Index = i + 1, Pair = a })
            .Where(a => Comparer.Compare(a.Pair.First, a.Pair.Second) == -1)
            .Sum(a => a.Index);
    }

    [GeneratedTest<int>(140, 21922)]
    public static int RunB(string[] lines)
    {
        var sep1 = Loader.ParseLine("[[2]]");
        var sep2 = Loader.ParseLine("[[6]]");

        var items = Loader.LoadItems(lines)
            .AppendRange(sep1, sep2);

        var sorted = items.Order().ToArray();

        var index1 = Array.IndexOf(sorted, sep1);
        var index2 = Array.IndexOf(sorted, sep2);

        return (index1 + 1) * (index2 + 1);
    }
}

record Pair(ItemArray First, ItemArray Second);

abstract record Item : IComparable<Item>
{
    public int CompareTo(Item? other)
        => Comparer.Compare(this, other!); //bang
}

record ItemValue(int Value) : Item;
record ItemArray(Item[] Items) : Item;
