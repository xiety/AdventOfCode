namespace A2022.Problem16;

public sealed class Solver1
{
    readonly int totalTime = 30;

    int availableWorkingVaults;

    //2.5 min
    public int Run(Graph graph)
    {
        var currentTime = 0;

        var parent = graph.Nodes.First(); //first must be AA

        availableWorkingVaults = graph.Nodes.Count(a => a.Rate > 0);

        var path = Linked.Empty<string>();
        var releases = Linked.Empty<int>();
        var released = Linked.Empty<Release>();

        var maximum = Recurse(path, releases, released, currentTime, parent, false);

        return maximum;
    }

    int globalMaximum = 0;

    private int Recurse(Linked<string> path, Linked<int> released, Linked<Release> releases, int currentTime, GraphNode parent, bool opened)
    {
        var maximum = 0;

        if (path.Value != parent.Name)
            path = path.AddBefore(parent.Name);

        if (currentTime >= totalTime - 1 || released.Count == availableWorkingVaults)
        {
            var m = CalculatePreasure(releases, totalTime);

            if (m > globalMaximum)
                globalMaximum = m;

            return m;
        }

        if (parent.Rate > 0 && !released.Contains(parent.Id))
        {
            var newReleased = released.AddBefore(parent.Id);
            var newReleases = releases.AddBefore(new(parent.Id, parent.Rate, currentTime + 1));

            maximum = Math.Max(maximum, Recurse(path, newReleased, newReleases, currentTime + 1, parent, true));
        }

        foreach (var child in parent.Connections)
            if (opened || child.Name != path.Next?.Value)
                maximum = Math.Max(maximum, Recurse(path, released, releases, currentTime + 1, child, false));

        return maximum;
    }

    private static int CalculatePreasure(Linked<Release> releases, int currentTime)
    {
        var p = releases;
        var res = 0;

        while (!p.End)
        {
            res += p.Value!.Rate * (currentTime - p.Value!.FromTime);
            p = p.Next!;
        }

        return res;
    }

    readonly record struct Release(int Id, int Rate, int FromTime);
}
