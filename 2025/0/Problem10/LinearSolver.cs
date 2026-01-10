namespace A2025.Problem10;

using ParityPatterns = ILookup<int, int>;

class LinearSolver(int[][] vectors, int[] target)
{
    public int[]? Run()
        => SolveStep(target, PrecomputeParityPatterns(ComputeBasis()))?.Solution;

    Result? SolveStep(int[] target, ParityPatterns parityPatterns)
    {
        if (target.All(a => a == 0))
            return new(0, new int[vectors.Length]);

        var candidates = parityPatterns[GetVectorParity(target)];

        var query =
            from mask in candidates
            let next = TryReduce(target, mask)
            where next is not null
            let res = SolveStep(next, parityPatterns)
            where res is not null
            let totalCost = CalcTotalCost(mask, res.Cost)
            select (TotalCost: totalCost, Mask: mask, res.Solution);

        return query
            .MinByOrNullable(a => a.TotalCost)
            .ApplyIfNotNull(a => new Result(a.TotalCost, Reconstruct(a.Solution, a.Mask)));
    }

    int[]? TryReduce(int[] target, int mask)
        => target.ToArray((a, i) => a - SumActive(i, mask)) is var diff && diff.All(a => a >= 0 && (a & 1) == 0)
            ? diff.ToArray(a => a >> 1)
            : null;

    int SumActive(int dim, int mask)
        => vectors
            .WhereIndex(i => ((mask >> i) & 1) != 0)
            .Sum(a => a.Count(b => b == dim));

    ParityPatterns PrecomputeParityPatterns(int[] basis)
        => Enumerable.Range(0, 1 << vectors.Length)
            .ToLookup(a => Enumerable.Range(0, vectors.Length)
                .Where(b => ((a >> b) & 1) != 0)
                .Aggregate(0, (acc, i) => acc ^ basis[i]));

    int[] ComputeBasis()
        => Enumerable.Range(0, vectors.Length)
            .ToArray(a => ComputeParity(1 << a));

    int ComputeParity(int mask)
        => Enumerable.Range(0, target.Length)
            .Sum(a => (SumActive(a, mask) & 1) << a);

    static int CalcTotalCost(int mask, int cost)
        => int.PopCount(mask) + (cost << 1);

    static int[] Reconstruct(int[] sub, int mask)
        => sub.ToArray((a, i) => (a << 1) + ((mask >> i) & 1));

    static int GetVectorParity(int[] v)
        => v.Select((a, i) => (a & 1) << i).Sum();

    record Result(int Cost, int[] Solution);
}
