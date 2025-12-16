using System.Text.RegularExpressions;

namespace A2023.Problem04;

public static class Solver
{
    [GeneratedTest<int>(13, 26426)]
    public static int RunA(string[] lines)
        => CompiledRegs.FromLinesRegex(lines)
            .Sum(CalcPoints);

    [GeneratedTest<int>(30, 6227972)]
    public static int RunB(string[] lines)
    {
        var cards = CompiledRegs.FromLinesRegex(lines);

        var calculates = cards.ToArray(a => (a.Number, Win: CalcWin(a), Copies: 1));

        foreach (var i in calculates.Length)
            foreach (var j in (i + 1)..Math.Min(i + 1 + calculates[i].Win, calculates.Length))
                calculates[j].Copies += calculates[i].Copies;

        return calculates.Sum(a => a.Copies);
    }

    static int CalcWin(Card card)
        => card.Left.Sum(a => card.Right.Contains(a) ? 1 : 0);

    static int CalcPoints(Card card)
    {
        var win = CalcWin(card);
        return win == 0 ? 0 : (int)Math.Pow(2, win - 1);
    }
}

record Card(int Number, int[] Left, int[] Right);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^Card\s+(?<{nameof(Card.Number)}>\d+):(?:\s+(?<{nameof(Card.Left)}>\d+))+ \|(?:\s+(?<{nameof(Card.Right)}>\d+))+$")]
    [MapTo<Card>]
    public static partial Regex Regex();
}
