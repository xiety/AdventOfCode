using System.Text.RegularExpressions;

using Advent.Common;

namespace A2023.Problem04;

public class Solver : IProblemSolver<int>
{
    public int RunA(string filename)
    {
        var cards = CompiledRegs.Regex().FromFile<Card>(filename);
        return cards.Select(CalcPoints).Sum();
    }

    public int RunB(string filename)
    {
        var cards = CompiledRegs.Regex().FromFile<Card>(filename);

        var calculates = cards.ToArray(a => (a.Number, Win: CalcWin(a), Copies: 1));

        for (var i = 0; i < calculates.Length; ++i)
            for (var j = i + 1; j < Math.Min(i + 1 + calculates[i].Win, calculates.Length); ++j)
                calculates[j].Copies += calculates[i].Copies;

        return calculates.Select(a => a.Copies).Sum();
    }

    static int CalcWin(Card card)
        => card.Left.Select(a => card.Right.Contains(a) ? 1 : 0).Sum();

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
    public static partial Regex Regex();
}
