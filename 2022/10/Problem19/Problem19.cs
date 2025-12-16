using System.Text.RegularExpressions;

namespace A2022.Problem19;

public static class Solver
{
    [GeneratedTest<int>(33, 1958)]
    public static int RunA(string[] lines)
    {
        var items = CompiledRegs.FromLinesRegex(lines);
        var calculator = new Calculator(24);
        return items.Sum(a => calculator.Calculate(a) * a.Number);
    }

    [GeneratedTest<int>(62 * 56, 4257)]
    public static int RunB(string[] lines)
    {
        var items = CompiledRegs.FromLinesRegex(lines);
        var calculator = new Calculator(32);
        return items.Take(3).Select(calculator.Calculate).Mul();
    }
}

record Item(int Number, int OreOreCost, int ClayOreCost, int ObsidianOreCost, int ObsidianClayCost, int GeodeOreCost, int GeodeObsidianCost);
record RobotsPack(int OreRobot, int ClayRobot, int ObsidianRobot, int GeodeRobot);
record Resources(int Ore, int Clay, int Obsidian, int Geode);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^Blueprint (?<{nameof(Item.Number)}>\d+): Each ore robot costs (?<{nameof(Item.OreOreCost)}>\d+) ore. Each clay robot costs (?<{nameof(Item.ClayOreCost)}>\d+) ore. Each obsidian robot costs (?<{nameof(Item.ObsidianOreCost)}>\d+) ore and (?<{nameof(Item.ObsidianClayCost)}>\d+) clay. Each geode robot costs (?<{nameof(Item.GeodeOreCost)}>\d+) ore and (?<{nameof(Item.GeodeObsidianCost)}>\d+) obsidian.$")]
    [MapTo<Item>]
    public static partial Regex Regex();
}
