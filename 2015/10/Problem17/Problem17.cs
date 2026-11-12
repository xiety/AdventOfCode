namespace A2015.Problem17;

public static class Solver
{
    [GeneratedTest<int>(4, 1638)]
    public static int RunA(string[] lines, bool isSample)
        => Recurse(lines.ToArray(int.Parse), [], 0, 0, isSample ? 25 : 150)
            .Count();

    [GeneratedTest<int>(3, 17)]
    public static int RunB(string[] lines, bool isSample)
        => Recurse(lines.ToArray(int.Parse), [], 0, 0, isSample ? 25 : 150)
            .MinAllBy(a => a.Length).Length;

    static IEnumerable<int[]> Recurse(int[] quantities, int[] path, int start, int sum, int target)
    {
        foreach (var i in start..quantities.Length)
        {
            var newSum = sum + quantities[i];

            if (newSum == target)
            {
                yield return path;
            }
            else if (newSum < target)
            {
                foreach (var child in Recurse(quantities, [.. path, i], i + 1, newSum, target))
                    yield return child;
            }
        }
    }
}
