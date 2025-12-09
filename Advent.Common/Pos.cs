using System.Text.RegularExpressions;

namespace System;

public readonly record struct Pos(int X, int Y)
{
    public override string ToString()
        => $"({X}, {Y})";

    public static Pos Zero { get; } = new(0, 0);

    public static Pos operator +(Pos a, Pos b)
        => new(a.X + b.X, a.Y + b.Y);

    public static Pos operator -(Pos a, Pos b)
        => new(a.X - b.X, a.Y - b.Y);

    public static Pos operator -(Pos a)
        => new(-a.X, -a.Y);

    public static Pos operator *(Pos a, int num)
        => new(a.X * num, a.Y * num);

    public static Pos operator /(Pos a, int num)
        => new(a.X / num, a.Y / num);

    public static Pos operator +(Pos a, int num)
        => new(a.X + num, a.Y + num);

    public static Pos operator -(Pos a, int num)
        => new(a.X - num, a.Y - num);

    public int ManhattanLength
        => Math.Abs(X) + Math.Abs(Y);

    public static Pos Min(Pos a, Pos b)
        => new(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y));

    public static Pos Max(Pos a, Pos b)
        => new(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y));

    public static Pos Parse(string text)
        => CompiledPosRegex.PosRegex().MapTo<Pos>(text);
}

public readonly record struct Pos3(int X, int Y, int Z)
{
    public static Pos3 Zero => new(0, 0, 0);

    public override string ToString()
        => $"({X}, {Y}, {Z})";

    public static Pos3 operator +(Pos3 a, Pos3 b)
        => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    public static Pos3 operator -(Pos3 a, Pos3 b)
        => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public static bool operator >=(Pos3 a, Pos3 b)
        => a.X >= b.X && a.Y >= b.Y && a.Z >= b.Z;

    public static Pos3 operator /(Pos3 a, int num)
        => new(a.X / num, a.Y / num, a.Z / num);

    public static bool operator <=(Pos3 a, Pos3 b)
        => a.X <= b.X && a.Y <= b.Y && a.Z <= b.Z;

    public int ManhattanLength
        => Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);

    public long LengthSquared
        => (long)X * X + (long)Y * Y + (long)Z * Z;

    public double Length
        => Math.Sqrt(LengthSquared);

    public static Pos3 Parse(string text)
        => CompiledPosRegex.Pos3Regex().MapTo<Pos3>(text);
}

public static class PosExtensions
{
    extension(Pos p)
    {
        public IEnumerable<Pos> EnumerateRay(Pos dir)
        {
            var current = p + dir;
            while (true)
            {
                yield return current;
                current += dir;
            }
        }

        public Pos Rotate(int degrees)
        {
            var steps = Math.Mod(degrees / 90, 4);
            return steps switch
            {
                1 => new Pos(p.Y, -p.X),
                2 => new Pos(-p.X, -p.Y),
                3 => new Pos(-p.Y, p.X),
                _ => p
            };
        }
    }
}

static partial class CompiledPosRegex
{
    [GeneratedRegex(@$"^(?<{nameof(Pos.X)}>-?\d+),(?<{nameof(Pos.Y)}>-?\d+)$")]
    public static partial Regex PosRegex();

    [GeneratedRegex(@$"^(?<{nameof(Pos3.X)}>-?\d+),(?<{nameof(Pos3.Y)}>-?\d+),(?<{nameof(Pos3.Z)}>-?\d+)$")]
    public static partial Regex Pos3Regex();
}
