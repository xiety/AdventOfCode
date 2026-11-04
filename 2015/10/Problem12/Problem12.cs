using System.Text.Json;
using System.Text.RegularExpressions;

namespace A2015.Problem12;

public static class Solver
{
    [GeneratedTest<int>(6, 119433)]
    public static int RunA(string[] lines)
        => CompiledRegs.Regex().Matches(lines[0]).Sum(a => int.Parse(a.Value));

    [GeneratedTest<int>(6, 119433)]
    public static int RunAJson(string[] lines)
    {
        using var doc = JsonDocument.Parse(lines[0]);
        return CalcElement(doc.RootElement, false);
    }

    [GeneratedTest<int>(4, 68466)]
    public static int RunB(string[] lines)
    {
        using var doc = JsonDocument.Parse(lines[0]);
        return CalcElement(doc.RootElement, true);
    }

    static int CalcElement(JsonElement node, bool doCheck)
        => node.ValueKind switch
        {
            JsonValueKind.Array => CalcArray(node, doCheck),
            JsonValueKind.Object => CalcObject(node, doCheck),
            JsonValueKind.Number => node.GetInt32(),
            _ => 0,
        };

    static int CalcObject(JsonElement parent, bool doCheck)
    {
        if (doCheck && CheckObject(parent))
            return 0;

        return parent.EnumerateObject()
            .Sum(a => CalcElement(a.Value, doCheck));
    }

    static int CalcArray(JsonElement node, bool doCheck)
        => node.EnumerateArray().Sum(a => CalcElement(a, doCheck));

    static bool CheckObject(JsonElement parent)
        => parent.EnumerateObject()
            .Any(a => a.Value.ValueKind == JsonValueKind.String && a.Value.GetString() == "red");
}

static partial class CompiledRegs
{
    [GeneratedRegex(@$"\-?[0-9]+")]
    public static partial Regex Regex();
}
