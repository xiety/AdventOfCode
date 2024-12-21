using Advent.Common;

namespace A2022.Problem13;

public class Solver : IProblemSolver<int>
{
    public int RunA(string filename)
    {
        var pairs = Loader.Load(filename).ToArray();

        var result = pairs
            .Select((a, i) => new { Index = i + 1, Pair = a })
            .Where(a => Comparer.Compare(a.Pair.First, a.Pair.Second) == -1)
            .Sum(a => a.Index);

        return result;
    }

    public int RunB(string filename)
    {
        var sep1 = Loader.ParseLine("[[2]]");
        var sep2 = Loader.ParseLine("[[6]]");

        var items = Loader.LoadItems(filename)
            .Append([sep1, sep2]);

        var sorted = items.Order().ToArray();

        var index1 = Array.IndexOf(sorted, sep1);
        var index2 = Array.IndexOf(sorted, sep2);

        var result = (index1 + 1) * (index2 + 1);

        return result;
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
