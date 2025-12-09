using System.Text.RegularExpressions;

namespace A2022.Problem15;

static class Loader
{
    public static Item[] Load(string fileName)
        => File.ReadAllLines(fileName)
              .Select(CompiledRegs.MapToRegex)
              .ToArray(a => new Item(new(a.SensorX, a.SensorY), new(a.BeaconX, a.BeaconY)));
}

record ParsedItem(int SensorX, int SensorY, int BeaconX, int BeaconY);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"Sensor at x=(?<{nameof(ParsedItem.SensorX)}>-?\d+), y=(?<{nameof(ParsedItem.SensorY)}>-?\d+): closest beacon is at x=(?<{nameof(ParsedItem.BeaconX)}>-?\d+), y=(?<{nameof(ParsedItem.BeaconY)}>-?\d+)")]
    [MapTo<ParsedItem>]
    public static partial Regex Regex();
}
