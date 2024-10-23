﻿using Advent.Common;

namespace A2022.Problem03;

public class Solver : IProblemSolver<int>
{
    public int RunA(string filename)
    {
        var lines = File.ReadAllLines(filename);

        var results = lines.Select(line =>
        {
            var type = FindType(line);
            var priority = FindPriority(type);

            return priority;
        });

        return results.Sum();
    }

    public int RunB(string filename)
    {
        var lines = File.ReadAllLines(filename);

        var chunks = lines
            .Chunk(3)
            .Select(a => FindBadge(a[0], a[1], a[2]))
            .Select(FindPriority);

        return chunks.Sum();
    }

    static char FindBadge(string line1, string line2, string line3)
    {
        foreach (var char1 in line1)
            foreach (var char2 in line2)
                if (char1 == char2)
                    foreach (var char3 in line3)
                        if (char1 == char2 && char1 == char3)
                            return char1;

        throw new NotSupportedException();
    }

    static char FindType(string line)
    {
        var len = line.Length;
        var half = len / 2;

        var a = line[..half];
        var b = line[half..];

        foreach (var la in a)
            foreach (var lb in b)
                if (la == lb)
                    return la;

        throw new NotSupportedException();
    }

    static int FindPriority(char type)
        => type switch
        {
            >= 'a' and <= 'z' => type - 'a' + 1,
            >= 'A' and <= 'Z' => type - 'A' + 27,
            _ => throw new NotImplementedException(),
        };
}