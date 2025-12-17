namespace A2024.Problem19;

public static class Solver
{
    [GeneratedTest<long>(6, 216)]
    public static long RunA(string[] lines)
    {
        var (towels, patterns) = LoadData(lines);
        return patterns.Count(a => Check(a, towels));
    }

    [GeneratedTest<long>(16, 603191454138773)]
    public static long RunB(string[] lines)
    {
        var (towels, patterns) = LoadData(lines);
        return patterns.Sum(a => Count(a, towels));
    }

    static bool Check(string pattern, string[] towels)
        => towels.Any(a => pattern.StartsWith(a)
               && a.Length <= pattern.Length
               && (a == pattern || Check(pattern[a.Length..], towels)));

    static long Count(string pattern, string[] towels)
        => Memoization.RunRecursive<string, long>(pattern,
            (memo, p) => towels
                .Where(a => a.Length <= p.Length && p.StartsWith(a))
                .Sum(a => a.Length == p.Length ? 1 : memo(p[a.Length..])));

    //static long CountDP(ReadOnlySpan<char> goal, string[] patterns)
    //{
    //    var dp = new long[goal.Length + 1];
    //    dp[0] = 1;

    //    for (var i = 1; i < dp.Length; ++i)
    //        foreach (var pattern in patterns)
    //            if (pattern.Length <= i && goal[(i - pattern.Length)..i].SequenceEqual(pattern))
    //                dp[i] += dp[i - pattern.Length];

    //    return dp[^1];
    //}

    static (string[], string[]) LoadData(string[] lines)
    {
        var towels = lines[0].Split(", ");
        var patterns = lines[2..];
        return (towels, patterns);
    }
}
