using System.Text.RegularExpressions;

using Advent.Common;

namespace A2022.Problem19;

public class Solver : IProblemSolver<int>
{
    public int RunA(string filename)
    {
        var items = LoadFile(filename);
        var calculator = new Calculator(24);
        return items.Select(a => calculator.Calculate(a) * a.Number).Sum();
    }

    public int RunB(string filename)
    {
        var items = LoadFile(filename);
        var calculator = new Calculator(32);
        return items.Take(3).Select(calculator.Calculate).Mul();
    }

    static Item[] LoadFile(string filename)
        => CompiledRegs.Regex().FromFile<Item>(filename);
}

record Item(int Number, int OreOreCost, int ClayOreCost, int ObsidianOreCost, int ObsidianClayCost, int GeodeOreCost, int GeodeObsidianCost);
record RobotsPack(int OreRobot, int ClayRobot, int ObsidianRobot, int GeodeRobot);
record Resources(int Ore, int Clay, int Obsidian, int Geode);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^Blueprint (?<{nameof(Item.Number)}>\d+): Each ore robot costs (?<{nameof(Item.OreOreCost)}>\d+) ore. Each clay robot costs (?<{nameof(Item.ClayOreCost)}>\d+) ore. Each obsidian robot costs (?<{nameof(Item.ObsidianOreCost)}>\d+) ore and (?<{nameof(Item.ObsidianClayCost)}>\d+) clay. Each geode robot costs (?<{nameof(Item.GeodeOreCost)}>\d+) ore and (?<{nameof(Item.GeodeObsidianCost)}>\d+) obsidian.$")]
    public static partial Regex Regex();
}
