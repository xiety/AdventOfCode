using System.Text.RegularExpressions;

using Dic = System.Collections.Generic.Dictionary<char, long>;
using MyFunc = System.Func<(char, char, int), System.Collections.Generic.Dictionary<char, long>>;

using Advent.Common;
using System.Text;

namespace A2021.Problem14;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var (firstPart, secondPart) = File.ReadAllLines(filename).Split(String.Empty);

        var rules = CompiledRegs.Regex().FromLines<Item>(secondPart)
            .ToDictionary(a => a.A, a => a.B);

        var seq = new LinkedList<char>(firstPart.First());

        var steps = 10;

        var template = new StringBuilder("AA");

        for (var step = 0; step < steps; ++step)
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
            while (node is not null && node.Next is not null);
        }

        var grouped = seq.GroupBy(a => a).ToArray();

        var min = grouped.Min(a => a.LongCount());
        var max = grouped.Max(a => a.LongCount());

        var result = max - min;

        return result;
    }

    public long RunB(string filename)
    {
        var (firstPart, secondPart) = File.ReadAllLines(filename).Split(String.Empty);

        var rules = CompiledRegs.Regex().FromLines<Item>(secondPart)
            .ToDictionary(a => (a.A[0], a.A[1]), a => a.B);

        var text = firstPart.First();

        var maxSteps = 40;

        var groups = new Dic();

        MyFunc memo = null!;

        memo = Memoization.Wrap((MyFunc)Recurse);

        for (var i = 0; i < text.Length - 1; ++i)
            groups = groups.Merge(memo((text[i], text[i + 1], 0)));

        var min = groups.Min(a => a.Value);
        var max = groups.Max(a => a.Value);

        var result = max - min;

        return result;

        Dic Recurse((char a, char b, int level) p)
        {
            if (p.level < maxSteps && rules.TryGetValue((p.a, p.b), out var insert))
            {
                var d1 = memo((p.a, insert, p.level + 1));
                var d2 = memo((insert, p.b, p.level + 1));

                return d1.Merge(d2);
            }
            else
            {
                return new() { [p.b] = 1 };
            }
        }
    }
}

public record Item(string A, char B);

public static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Item.A)}>\w+) -> (?<{nameof(Item.B)}>\w+)$")]
    public static partial Regex Regex();
}
