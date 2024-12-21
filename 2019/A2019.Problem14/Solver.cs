using System.Text.RegularExpressions;

using Advent.Common;

namespace A2019.Problem14;

public class Solver : ISolver<long>
{
    public long RunA(string[] lines, bool isSample)
    {
        var items = LoadData(lines);
        return Recurse(items, [], "FUEL", 1, 0);
    }

    static long Recurse(Item[] items, Dictionary<string, long> bag, string outputName, long outputNum, int level)
    {
        if (outputName == "ORE")
            return outputNum;

        var item = items.First(b => b.Output.Name == outputName);

        var requiredNum = outputNum;

        if (bag.TryGetValue(outputName, out var inBag))
        {
            requiredNum = Math.Max(0, requiredNum - inBag);
            bag[outputName] = Math.Max(0, inBag - outputNum);
        }

        if (requiredNum == 0)
            return 0;

        var howManyIterations = (requiredNum + item.Output.Num - 1) / item.Output.Num;
        var waste = (howManyIterations * item.Output.Num) - requiredNum;

        if (waste > 0)
            bag.AddOrReplace(outputName, waste, a => a + waste);

        var result = 0L;

        //bag is mutated in every iteration
        for (var i = 0; i < howManyIterations; ++i)
            result += item.Input.Sum(a => Recurse(items, bag, a.Name, a.Num, level + 1));

        return result;
    }

    static Item[] LoadData(string[] lines)
        => CompiledRegs.Regex().FromLines<ItemRaw>(lines)
            .ToArray(a => new Item([.. a.InNum.Zip(a.InName).Select(b => new Info(b.First, b.Second))], new(a.OutNum, a.OutName)));
}

record ItemRaw(int[] InNum, string[] InName, int OutNum, string OutName);
record Info(int Num, string Name);
record Item(Info[] Input, Info Output);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^((?<{nameof(ItemRaw.InNum)}>\d+)\s(?<{nameof(ItemRaw.InName)}>[A-Z]+)(,\s)?)+\s=\>\s(?<{nameof(ItemRaw.OutNum)}>\d+)\s(?<{nameof(ItemRaw.OutName)}>[A-Z]+)$")]
    public static partial Regex Regex();
}
