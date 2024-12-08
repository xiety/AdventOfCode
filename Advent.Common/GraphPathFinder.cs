namespace Advent.Common.Graph;

public static class GraphPathFinder
{
    public static Dictionary<GraphNode, int> CalculateStar(GraphNode[] nodes, GraphNode start, GraphNode end)
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
                    var oldStar = star.GetValueOrDefault(newStep, -1);

                    var newStar = currentDistance + 1;

                    if (oldStar == -1 || oldStar > newStar)
                    {
                        star[newStep] = newStar;

                        if (newStep == end)
                            return star;

                        newSteps.Add(newStep);
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

public class GraphNode
{
    public required string Name { get; init; }
    public List<GraphNode> Connections { get; } = [];
}

public class Graph
{
    public required GraphNode Start { get; init; }
    public required GraphNode End { get; init; }
}
