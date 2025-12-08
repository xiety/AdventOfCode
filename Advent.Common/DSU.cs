namespace Advent.Common;

public sealed class Dsu(int n)
{
    readonly int[] parent = Enumerable.Range(0, n).ToArray();
    readonly int[] size = Enumerable.Repeat(1, n).ToArray();
    readonly int[] rank = new int[n];

    public int Count { get; private set; } = n;

    public IEnumerable<int> Sizes =>
        Enumerable.Range(0, parent.Length)
                  .Select(Find)
                  .Distinct()
                  .Select(root => size[root]);

    public int Find(int x)
        => parent[x] == x ? x : parent[x] = Find(parent[x]);

    public bool Union(int x, int y)
    {
        var xr = Find(x);
        var yr = Find(y);

        if (xr == yr)
            return false;

        if (rank[xr] < rank[yr])
            (xr, yr) = (yr, xr);

        parent[yr] = xr;
        size[xr] += size[yr];

        if (rank[xr] == rank[yr])
            rank[xr]++;

        Count--;

        return true;
    }
}
