using System.Text.RegularExpressions;

using Advent.Common;

namespace A2020.Problem16;

public static class Solver
{
    [GeneratedTest<int>(71, 22000)]
    public static int RunA(string[] lines)
    {
        var data = LoadData(lines);
        return data.NearbyTickets
            .SelectMany(a => a.Where(b => !CheckRegions(data.Regions, b)))
            .Sum();
    }

    [GeneratedTest<long>(102, 410460648673)]
    public static long RunB(string[] lines, bool isSample)
    {
        var data = LoadData(lines);

        var positions = FindPositions(data);

        if (isSample)
        {
            return data.Regions.Index()
                .Join(positions.Index(), a => a.Item.Name, b => b.Item, (a, b) => (a.Index, b.Index))
                .OrderBy(a => a.Item1)
                .Select(a => a.Item2)
                .Aggregate(0, (acc, a) => acc * 10 + a);
        }
        else
        {
            return positions.Index().Where(a => a.Item.Contains("departure"))
                .Mul(a => (long)data.YourTicket[a.Index]);
        }
    }

    static string[] FindPositions(Data data)
    {
        var tickets = data.NearbyTickets
            .Where(a => a.All(b => CheckRegions(data.Regions, b)))
            .Append(data.YourTicket)
            .ToArray();

        return Memoization.RunRecursive<string, int, string[]?>(new('1', data.Regions.Length), 0,
            (memo, available, depth) =>
            {
                foreach (var i in data.Regions.Length)
                {
                    if (available[i] == '0')
                        continue;

                    var region = data.Regions[i];
                    var good = tickets.All(a => CheckRegion(region, a[depth]));

                    if (good)
                    {
                        if (depth == data.Regions.Length - 1)
                            return [region.Name];

                        var newAvailable = available[..i] + '0' + available[(i + 1)..];
                        var child = memo(newAvailable, depth + 1);
                        if (child is not null)
                            return [region.Name, .. child];
                    }
                }

                return null;
            })!;
    }

    static bool CheckRegions(ItemRegion[] regions, int n)
        => regions.Any(a => CheckRegion(a, n));

    static bool CheckRegion(ItemRegion region, int n)
        => (region.From1 <= n && region.To1 >= n) || (region.From2 <= n && region.To2 >= n);

    static Data LoadData(string[] lines)
    {
        var parts = lines.SplitBy(string.Empty).ToArray();
        var regions = parts[0].ToArray(CompiledRegs.MapToRegex);
        var yours = parts[1][1].Split(",").ToArray(int.Parse);
        var tickets = parts[2].Skip(1).ToArray(a => a.Split(",").ToArray(int.Parse));
        return new(regions, yours, tickets);
    }
}

record Data(ItemRegion[] Regions, int[] YourTicket, int[][] NearbyTickets);
record struct ItemRegion(string Name, int From1, int To1, int From2, int To2);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(ItemRegion.Name)}>.+)\: (?<{nameof(ItemRegion.From1)}>\d+)-(?<{nameof(ItemRegion.To1)}>\d+) or (?<{nameof(ItemRegion.From2)}>\d+)-(?<{nameof(ItemRegion.To2)}>\d+)$")]
    [MapTo<ItemRegion>]
    public static partial Regex Regex();
}
