using Advent.Common;

namespace A2024.Problem19;

public class Solver : ISolver<long>
{
    public long RunA(string[] lines, bool isSample)
    {
        var (towels, patterns) = LoadData(lines);
        return patterns.Count(a => Check(a, towels));
    }

    public long RunB(string[] lines, bool isSample)
    {
        var (towels, patterns) = LoadData(lines);
        return patterns.Sum(a => Count(a, towels));
    }

    static bool Check(string pattern, string[] towels)
        => towels
        .Any(a => pattern.StartsWith(a)
               && a.Length <= pattern.Length
               && (a == pattern || Check(pattern[a.Length..], towels)));

    static long Count(string pattern, string[] towels)
    {
        Func<string, long> memo = null!;
        //trick from https://github.com/dotnet/csharplang/discussions/129#discussioncomment-2455671
        var recurse = Recurse;
        memo = Memoization.Wrap(recurse);
        return Recurse(pattern);

        long Recurse(string pattern)
            => towels
            .Where(a => a.Length <= pattern.Length && pattern.StartsWith(a))
            .Sum(a => a.Length == pattern.Length ? 1 : memo(pattern[a.Length..]));
    }

    //https://www.reddit.com/r/adventofcode/comments/1hhvroq/2024_day_19_python_dynamic_programing_solution/
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
        var towels = lines[0].Split(", ").ToArray();
        var patterns = lines[2..];
        return (towels, patterns);
    }
}
