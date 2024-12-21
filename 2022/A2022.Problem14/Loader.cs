using System.Text.RegularExpressions;

namespace A2022.Problem14;

static class Loader
{
    public static List<Pos[]> Load(string filename)
        => File.ReadAllLines(filename).Select(ParseLine).ToList();

    static Pos[] ParseLine(string line)
        => line.Split(" -> ")
               .ToArray(CompiledRegs.Regex().MapTo<Pos>);
}

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Pos.X)}>\d+),(?<{nameof(Pos.Y)}>\d+)$")]
    public static partial Regex Regex();
}
