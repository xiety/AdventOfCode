using System.Text.RegularExpressions;

namespace A2022.Problem14;

static class Loader
{
    public static List<Pos[]> Load(string filename)
        => File.ReadAllLines(filename).Select(ParseLine).ToList();

    private static Pos[] ParseLine(string line)
        => line.Split(" -> ")
               .Select(CompiledRegs.Regex().MapTo<Pos>)
               .ToArray();
}

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Pos.X)}>\d+),(?<{nameof(Pos.Y)}>\d+)$")]
    public static partial Regex Regex();
}
