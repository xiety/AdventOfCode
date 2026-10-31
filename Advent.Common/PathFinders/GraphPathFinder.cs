namespace Advent.Common;

public static class GraphPathFinder
{
    public static Dictionary<GraphNode, int> CalculateStar(GraphNode start, GraphNode end)
    {
        var star = new Dictionary<GraphNode, int> { [start] = 0 };

        var currentSteps = new List<GraphNode> { start };
        var newSteps = new List<GraphNode>();

        do
        {
            foreach (var currentStep in currentSteps)
            {
                var currentDistance = star[currentStep];

                foreach (var newStep in currentStep.Connections)
                {
                    var oldStar = star.GetValueOrDefault(newStep.Destination, -1);

                    var newStar = currentDistance + newStep.Distance;

                    if (oldStar == -1 || oldStar > newStar)
                    {
                        star[newStep.Destination] = newStar;

                        if (newStep.Destination == end)
                            return star;

                        newSteps.Add(newStep.Destination);
                    }
                }
            }

            if (newSteps.Count == 0)
                return star;

            (currentSteps, newSteps) = (newSteps, currentSteps);
            newSteps.Clear();
        }
        while (true);
    }
}

public record GraphNodeConnection(GraphNode Destination, int Distance);

public class GraphNode(string name)
{
    public string Name { get; } = name;
    public List<GraphNodeConnection> Connections { get; } = [];

    public static GraphNode Create(string name)
        => new(name);
}
