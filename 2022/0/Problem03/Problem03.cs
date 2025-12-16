using Advent.Common;

namespace A2022.Problem03;

public static class Solver
{
    [GeneratedTest<int>(157, 7863)]
    public static int RunA(string[] lines)
        => lines
            .Sum(line => FindPriority(FindType(line)));

    [GeneratedTest<int>(70, 2488)]
    public static int RunB(string[] lines)
        => lines
            .Chunk(3)
            .Select(a => FindBadge(a[0], a[1], a[2]))
            .Sum(FindPriority);

    static char FindBadge(string line1, string line2, string line3)
    {
        var items =
            from char1 in line1
            from char2 in line2
            where char1 == char2
            from char3 in line3
            where char1 == char2 && char1 == char3
            select char1;

        return items.First();
    }

    static char FindType(string line)
    {
        var len = line.Length;
        var half = len / 2;

        var a = line[..half];
        var b = line[half..];

        var items =
            from la in a
            from lb in b
            where la == lb
            select la;

        return items.First();
    }

    static int FindPriority(char type)
        => type switch
        {
            >= 'a' and <= 'z' => type - 'a' + 1,
            >= 'A' and <= 'Z' => type - 'A' + 27,
            _ => throw new NotImplementedException(),
        };
}
