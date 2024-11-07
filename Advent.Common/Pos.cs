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

    public int ManhattanLength
        => Math.Abs(X) + Math.Abs(Y);
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

    public static bool operator <=(Pos3 a, Pos3 b)
        => a.X <= b.X && a.Y <= b.Y && a.Z <= b.Z;
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

    public bool Intersects(Pos3 pos)
        => pos >= From && pos <= To;

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
        => (long)Width * Height;

    public int Width
        => To.X - From.X + 1;

    public int Height
        => To.Y - From.Y + 1;

    public bool Intersects(Pos pos)
        => From.X <= pos.X && To.X >= pos.X && From.Y <= pos.Y && To.Y >= pos.Y;

    public static Rect CreateBoundingBox(IReadOnlyCollection<Pos> items)
    {
        var minX = items.Select(a => a.X).Min();
        var maxX = items.Select(a => a.X).Max();
        var minY = items.Select(a => a.Y).Min();
        var maxY = items.Select(a => a.Y).Max();

        return new(new(minX, minY), new(maxX, maxY));
    }
}
