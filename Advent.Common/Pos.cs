namespace System;

public readonly record struct Pos(int X, int Y)
{
    public static Pos Zero { get; } = new(0, 0);

    public static Pos operator +(Pos a, Pos b)
        => new(a.X + b.X, a.Y + b.Y);

    public static Pos operator -(Pos a, Pos b)
        => new(a.X - b.X, a.Y - b.Y);

    public static Pos operator -(Pos a)
        => new(-a.X, -a.Y);
}

public readonly record struct Pos3(int X, int Y, int Z)
{
    public override string ToString()
        => $"({X}, {Y}, {Z})";

    public static Pos3 operator +(Pos3 a, Pos3 b)
        => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    public static Pos3 operator -(Pos3 a, Pos3 b)
        => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public static bool operator >=(Pos3 a, Pos3 b)
        => a.X >= b.X && a.Y >= b.Y && a.Z >= b.Z;

    public static bool operator <=(Pos3 a, Pos3 b)
        => a.X <= b.X && a.Y <= b.Y && a.Z <= b.Z;

    //public static bool operator >(Pos3 a, Pos3 b)
    //    => a.X > b.X && a.Y > b.Y && a.Z > b.Z;

    //public static bool operator <(Pos3 a, Pos3 b)
    //    => a.X < b.X && a.Y < b.Y && a.Z < b.Z;

    //if (from1.X > to2.X || from1.Y > to2.Y || from1.Z > to2.Z)
    //    return false;

    //if (from2.X > to1.X || from2.Y > to1.Y || from2.Z > to1.Z)
    //    return false;
}

public readonly record struct Rect3(Pos3 From, Pos3 To)
{
    public long Volume
    {
        get
        {
            var diag = new Pos3(To.X - From.X + 1, To.Y - From.Y + 1, To.Z - From.Z + 1);
            return (long)diag.X * (long)diag.Y * (long)diag.Z;
        }
    }

    public bool Intersects(Rect3 rect)
        => From <= rect.To && rect.From <= To;

    public Rect3? Intersection(Rect3 rect)
    {
        if (!Intersects(rect))
            return null;

        return new(
            new(Math.Max(From.X, rect.From.X), Math.Max(From.Y, rect.From.Y), Math.Max(From.Z, rect.From.Z)),
            new(Math.Min(To.X, rect.To.X), Math.Min(To.Y, rect.To.Y), Math.Min(To.Z, rect.To.Z))
        );
    }
}

public readonly record struct Rect(Pos From, Pos To)
{
    public long Volume
        => (To.X - From.X + 1L) * (To.Y - From.Y + 1L);
}
