using System.Text.RegularExpressions;

namespace A2022.Problem14;

static class Loader
{
    public static List<Pos[]> Load(string filename)
        => File.ReadAllLines(filename).ToList(ParseLine);

    static Pos[] ParseLine(string line)
        => line.Split(" -> ")
               .ToArray(Pos.Parse);
}
