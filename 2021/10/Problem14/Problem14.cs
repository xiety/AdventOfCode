using System.Text;
using System.Text.RegularExpressions;

using Dic = System.Collections.Generic.Dictionary<char, long>;

namespace A2021.Problem14;

public static class Solver
{
    [GeneratedTest<long>(1588, 3118)]
    public static long RunA(string[] lines)
    {
        var (firstPart, secondPart) = lines.SplitBy(String.Empty);

        var rules = CompiledRegs.FromLinesRegex(secondPart)
            .ToDictionary(a => a.A, a => a.B);

        var seq = new LinkedList<char>(firstPart[0]);

        const int steps = 10;

        var template = new StringBuilder("AA");

        foreach (var _ in steps)
        {
            var node = seq.First!;

            do
            {
                template[0] = node.Value;
                template[1] = node.Next!.Value;

                if (rules.TryGetValue(template.ToString(), out var insert))
                    node = seq.AddAfter(node, insert);

                node = node.Next;
            }
            while (node?.Next is not null);
        }

        var grouped = seq.GroupBy(a => a).ToArray();

        var min = grouped.Min(a => a.LongCount());
        var max = grouped.Max(a => a.LongCount());

        return max - min;
    }

    [GeneratedTest<long>(2188189693529, 4332887448171)]
    public static long RunB(string[] lines)
    {
        var (firstPart, secondPart) = lines.SplitBy(String.Empty);

        var rules = CompiledRegs.FromLinesRegex(secondPart)
            .ToDictionary(a => (a.A[0], a.A[1]), a => a.B);

        var text = firstPart[0];

        const int maxSteps = 40;

        var groups = new Dic();

        var memo = Memoization.WrapRecursive<(char a, char b, int level), Dic>((memo, p) =>
        {
            if (p.level < maxSteps && rules.TryGetValue((p.a, p.b), out var insert))
            {
                var d1 = memo((p.a, insert, p.level + 1));
                var d2 = memo((insert, p.b, p.level + 1));

                return d1.Merge(d2);
            }

            return new() { [p.b] = 1 };
        });

        foreach (var i in text.Length - 1)
            groups = groups.Merge(memo((text[i], text[i + 1], 0)));

        var min = groups.Min(a => a.Value);
        var max = groups.Max(a => a.Value);

        return max - min;
    }
}

record Item(string A, char B);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Item.A)}>\w+) -> (?<{nameof(Item.B)}>\w+)$")]
    [MapTo<Item>]
    public static partial Regex Regex();
}
