namespace A2025.Problem10;

static class LinearSolver
{
    public static int[]? Run(Item item)
    {
        var parityPatterns = PrecomputeParityPatterns(item);

        return Memoization.RunRecursive<int[], Result>(
            item.Jolts,
            (recurse, target) => SolveRecursive(recurse, target, item, parityPatterns)).Solution;
    }

    static Dictionary<int, List<int>> PrecomputeParityPatterns(Item item)
        => Enumerable.Range(0, 1 << item.Buttons.Length)
            .GroupBy(mask => ComputeParity(mask, item))
            .ToDictionary(a => a.Key, a => a.ToList());

    static int ComputeParity(int buttonMask, Item item)
        => Enumerable.Range(0, item.Jolts.Length)
            .Select(j => item.Buttons
                .Select((buttons, b) => ((buttonMask >> b) & 1) * buttons.Count(a => a == j))
                .Sum() % 2)
            .Select((bit, i) => bit << i)
            .Sum();

    static Result SolveRecursive(Func<int[], Result> recurse, int[] target, Item item, Dictionary<int, List<int>> parityPatterns)
    {
        if (target.All(a => a == 0))
            return new(0, new int[item.Buttons.Length]);

        var parity = target
            .Index()
            .Where(a => (a.Item & 1) == 1)
            .Aggregate(0, (acc, a) => acc | (1 << a.Index));

        if (!parityPatterns.TryGetValue(parity, out var patterns))
            return new(null, null);

        return patterns
            .Select(pattern => (pattern, newTarget: ApplyPattern(pattern, target, item)))
            .Where(a => a.newTarget is not null)
            .Select(a => recurse(a.newTarget!).Apply(b => (
                    Pattern: a.pattern,
                    SubResult: b,
                    TotalCost: b.Cost is int c ?
                        int.PopCount(a.pattern) + 2 * c :
                        (int?)null)))
            .Where(a => a.TotalCost is int && a.SubResult.Solution is not null)
            .Select(a => new Result(
                a.TotalCost!.Value,
                CombineSolution(a.Pattern, a.SubResult.Solution!)
            ))
            .OrderBy(a => a.Cost)
            .FirstOrDefault();
    }

    static int[]? ApplyPattern(int pattern, int[] target, Item item)
    {
        var newTarget = Enumerable.Range(0, target.Length)
            .ToArray(j => target[j] - Enumerable.Range(0, item.Buttons.Length)
                .Count(b => ((pattern >> b) & 1) == 1 && item.Buttons[b].Contains(j)));

        if (newTarget.Any(a => a < 0 || (a & 1) == 1))
            return null;

        return newTarget.ToArray(x => x / 2);
    }

    static int[] CombineSolution(int pattern, int[] subSolution)
        => subSolution
            .ToArray((value, b) => ((pattern >> b) & 1) == 1 ? value * 2 + 1 : value * 2);

    record struct Result(int? Cost, int[]? Solution);
}
