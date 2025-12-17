namespace A2024.Problem23;

public static class Solver
{
    [GeneratedTest<long>(7, 1098)]
    public static long RunA(string[] lines)
    {
        var (connections, nodes) = LoadData(lines);

        var query = from connection in connections
                    from node in nodes
                    where node != connection.Item1
                       && node != connection.Item2
                    where connections.Contains((connection.Item1, node))
                       && connections.Contains((connection.Item2, node))
                    let array = (string[])[connection.Item1, connection.Item2, node]
                    where array.Any(a => a.StartsWith('t'))
                    select array.Order().ToTuple3();

        return query.Distinct().Count();
    }

    [GeneratedTest<string>("co,de,ka,ta", "ar,ep,ih,ju,jx,le,ol,pk,pm,pp,xf,yu,zg")]
    public static string RunB(string[] lines)
    {
        var (connections, nodes) = LoadData(lines);
        var result = Recurse(nodes, connections, [], 0);
        return result.Order().ToArray().StringJoin(",");
    }

    static string[] Recurse(string[] nodes, HashSet<(string, string)> connections, string[] current, int start)
        => nodes
            .Skip(start).Index()
            .Where(a => current.All(b => connections.Contains((b, a.Item))))
            .Select(a => Recurse(nodes, connections, [.. current, a.Item], a.Index + start + 1))
            .MaxBy(a => a.Length) ?? current;

    static (HashSet<(string, string)>, string[]) LoadData(string[] lines)
    {
        var list = lines.ToArray(a => a.Split('-'));
        var connections = new HashSet<(string, string)>(list.Select(a => (a[0], a[1])).Concat(list.Select(a => (a[1], a[0]))));
        var nodes = list.Select(a => a[0]).Concat(list.Select(a => a[1])).Distinct().Order().ToArray();
        return (connections, nodes);
    }
}
