using Advent.Common;

namespace A2024.Problem11;

using Cache = Dictionary<long, TreeNode>;
using CacheCount = Dictionary<(long, int), long>;

public class Solver : ISolver<long>
{
    public long RunA(string[] lines, bool isSample)
        => Run(lines, 25);

    public long RunB(string[] lines, bool isSample)
        => Run(lines, 75);

    static long Run(string[] lines, int total)
    {
        var items = LoadData(lines);

        var tree = CreateRoot(items);

        for (var i = 0; i < total; ++i)
            Change(tree, i);

        return Count(tree, total);
    }

    static void Change(TreeNode tree, int iteration)
    {
        if (tree.Calculation == iteration)
            return;

        tree.Calculation = iteration;

        foreach (var subtree in tree.Children)
        {
            if (subtree.Children.Length == 0)
                subtree.SetChildren(CreateChildren(subtree.Value));
            else
                Change(subtree, iteration);
        }
    }

    static long[] CreateChildren(long n)
    {
        if (n == 0)
        {
            return [1];
        }
        else
        {
            var ns = n.ToString();

            if (ns.Length % 2 == 0)
            {
                var (n1, n2) = Split(ns);
                return [n1, n2];
            }
            else
            {
                var n1 = n * 2024;
                return [n1];
            }
        }
    }

    static long Count(TreeNode tree, int total)
        => Count([], tree, total, 0);

    static long Count(CacheCount cache, TreeNode tree, int iteration, int level)
    {
        if (level >= iteration)
            return tree.Children.Length;

        var left = iteration - level;

        if (cache.TryGetValue((tree.Value, left), out var result))
            return result;

        var c = tree.Children.Select(a => Count(cache, a, iteration, level + 1)).Sum();
        cache.Add((tree.Value, left), c);

        return c;
    }

    private static TreeNode CreateRoot(IEnumerable<long> items)
    {
        var root = new TreeNode(-1, []);
        root.SetChildren(items);
        return root;
    }

    static (long, long) Split(string ns)
    {
        var half = ns.Length / 2;
        var n1 = long.Parse(ns[..half]);
        var n2 = long.Parse(ns[half..]);
        return (n1, n2);
    }

    static IEnumerable<long> LoadData(string[] lines)
        => lines[0].Split(' ').Select(long.Parse);
}

class TreeNode(long value, Cache cache)
{
    public TreeNode[] Children { get; set; } = [];
    public long Value => value;
    public int Calculation { get; set; } = -1;

    public void SetChildren(params IEnumerable<long> values)
        => Children = values.Select(FindOrCreate).ToArray();

    TreeNode FindOrCreate(long value)
    {
        if (cache.TryGetValue(value, out var tree))
            return tree;

        tree = new TreeNode(value, cache);
        cache.Add(value, tree);

        return tree;
    }
}
