﻿using System.Text.RegularExpressions;

using Advent.Common;

namespace A2024.Problem13;

public class Solver : ISolver<long>
{
    public long RunA(string[] lines, bool isSample)
    {
        var items = LoadData(lines);
        return Run(items);
    }

    public long RunB(string[] lines, bool isSample)
    {
        var offset = 10000000000000L;
        var items = LoadData(lines).Select(a => a with { X = a.X + offset, Y = a.Y + offset });
        return Run(items);
    }

    static long Run(IEnumerable<Item> items)
        => items.Select(Calc).OfType<long>().Sum();

    static long? Calc(Item item)
    {
        var ka = (long)item.Ax * item.By - item.Ay * item.Bx;
        var ra = item.X * item.By - item.Y * item.Bx;

        if (ra % ka != 0)
            return null;

        var a = ra / ka;
        var b = (item.X - a * item.Ax) / item.Bx;

        return 3 * a + b;
    }

    static Item[] LoadData(string[] lines)
        => lines.Split(String.Empty).ToArray(a =>
        {
            var line1 = CompiledRegs.Line1().MapTo<ItemLine1>(a[0]);
            var line2 = CompiledRegs.Line2().MapTo<ItemLine2>(a[1]);
            var line3 = CompiledRegs.Line3().MapTo<ItemLine3>(a[2]);
            return new Item(line1.Ax, line1.Ay, line2.Bx, line2.By, line3.X, line3.Y);
        });
}

record Item(int Ax, int Ay, int Bx, int By, long X, long Y);

record ItemLine1(int Ax, int Ay);
record ItemLine2(int Bx, int By);
record ItemLine3(int X, int Y);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^Button A: X\+(?<{nameof(Item.Ax)}>\d+), Y\+(?<{nameof(Item.Ay)}>\d+)$")]
    public static partial Regex Line1();
    [GeneratedRegex(@$"^Button B: X\+(?<{nameof(Item.Bx)}>\d+), Y\+(?<{nameof(Item.By)}>\d+)$")]
    public static partial Regex Line2();
    [GeneratedRegex(@$"^Prize: X=(?<{nameof(Item.X)}>\d+), Y=(?<{nameof(Item.Y)}>\d+)$")]
    public static partial Regex Line3();
}
