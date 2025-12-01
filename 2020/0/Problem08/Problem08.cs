using System.Text.RegularExpressions;

using Advent.Common;

namespace A2020.Problem08;

public static class Solver
{
    [GeneratedTest<int>(5, 1451)]
    public static int RunA(string[] lines)
        => Simulate(LoadItems(lines)).Acc;

    [GeneratedTest<int>(8, 1160)]
    public static int RunB(string[] lines)
    {
        var items = LoadItems(lines);

        return Enumerable.Range(0, items.Length)
            .Where(i => items[i].Op is Ops.Jmp or Ops.Nop)
            .AsParallel()
            .Select(i => Simulate(new ProgramPatched(items, i)))
            .First(result => result.Finished)
            .Acc;
    }

    static (bool Finished, int Acc) Simulate(Program program)
    {
        var visited = new HashSet<int>();
        var (pos, acc) = (0, 0);

        while (pos < program.Length && visited.Add(pos))
        {
            var item = program[pos];

            (pos, acc) = item.Op switch
            {
                Ops.Nop => (pos + 1, acc),
                Ops.Acc => (pos + 1, acc + item.Value),
                Ops.Jmp => (pos + item.Value, acc),
            };
        }

        return (pos >= program.Length, acc);
    }

    static Item[] LoadItems(string[] lines)
        => CompiledRegs.FromLinesRegex(lines);
}

class Program(Item[] items)
{
    public virtual Item this[int index] => items[index];
    public virtual int Length => items.Length;
    public static implicit operator Program(Item[] items) => new(items);
}

sealed class ProgramPatched(Item[] items, int flipIndex) : Program(items)
{
    public override Item this[int index]
    {
        get
        {
            var item = base[index];
            if (flipIndex == index)
                item = item with { Op = item.Op == Ops.Jmp ? Ops.Nop : Ops.Jmp };
            return item;
        }
    }
}

enum Ops { Nop, Acc, Jmp }

record Item(Ops Op, int Value);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(Item.Op)}>.+) (?<{nameof(Item.Value)}>[\+\-]\d+)$")]
    [MapTo<Item>]
    public static partial Regex Regex();
}
