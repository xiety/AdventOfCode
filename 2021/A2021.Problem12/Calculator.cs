namespace A2021.Problem12;

public class Calculator(Graph graph, bool single)
{
    public int Calculate()
    {
        var path = new LinkedList<GraphNode>();

        return Recurse(graph.Start, 0, path);
    }

    private int Recurse(GraphNode parent, int level, LinkedList<GraphNode> path)
    {
        if (parent == graph.End)
            return 1;

        if (single && parent.CaveType == CaveType.Small && path.Contains(parent))
            return 0;

        if (parent == graph.Start && level > 0)
            return 0;

        if (parent.CaveType == CaveType.Small)
        {
            var already = path.Contains(parent);

            if (already)
            {
                var forbidden = path.Where(a => a.CaveType == CaveType.Small)
                    .GroupBy(a => a).Any(a => a.Count() > 1);

                if (forbidden)
                    return 0;
            }
        }

        path.AddLast(parent);

        var result = parent.Connections
            .Sum(a => Recurse(a, level + 1, path));

        path.RemoveLast();

        return result;
    }
}

public class GraphNode
{
    public required string Name { get; init; }
    public required CaveType CaveType { get; init; }
    public List<GraphNode> Connections { get; } = [];
}

public class Graph
{
    public required GraphNode Start { get; init; }
    public required GraphNode End { get; init; }
}

public enum CaveType
{
    StartOrEnd,
    Small,
    Large,
}
