namespace A2022.Problem16;

public static class Grapher
{
    public static Graph CreateGraph(Item[] items)
    {
        var graph = new Graph();

        var indexed = new List<string>();
        var index = 0;

        foreach (var item in items.OrderBy(a => a.Valve))
        {
            var node = new GraphNode { Id = index, Name = item.Valve, Rate = item.Rate };
            graph.Nodes.Add(node);

            indexed.Add(item.Valve);
            index++;
        }

        foreach (var item in items)
        {
            var node = graph.Nodes.First(a => a.Id == indexed.IndexOf(item.Valve));

            foreach (var childName in item.Targets.Reverse())
            {
                var childNode = graph.Nodes.First(a => a.Id == indexed.IndexOf(childName));

                //childNode.Connections.Add(node);
                node.Connections.Add(childNode);
            }
        }

        return graph;
    }
}

public class GraphNode
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required int Rate { get; init; }

    public List<GraphNode> Connections { get; } = [];
}

public class Graph
{
    public List<GraphNode> Nodes { get; } = [];
}
