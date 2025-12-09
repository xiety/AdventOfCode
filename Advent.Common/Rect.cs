namespace Advent.Common;

public readonly record struct Rect(Pos P1, Pos P2)
{
    public Pos From => Pos.Min(P1, P2);
    public Pos To => Pos.Max(P1, P2);

    public long Area
        => (long)Width * Height;

    public int Width
        => To.X - From.X + 1;

    public int Height
        => To.Y - From.Y + 1;

    public bool Intersects(Pos pos)
        => From.X <= pos.X && To.X >= pos.X && From.Y <= pos.Y && To.Y >= pos.Y;

    public bool Intersects(Line edge)
        => edge.IsVertical
            ? edge.Start.X > From.X && edge.Start.X < To.X &&
              !(From.Y >= edge.Max.Y || To.Y <= edge.Min.Y)
            : edge.Start.Y > From.Y && edge.Start.Y < To.Y &&
              !(From.X >= edge.Max.X || To.X <= edge.Min.X);

    public static Rect CreateBoundingBox(IReadOnlyCollection<Pos> items)
    {
        var minX = items.Min(a => a.X);
        var maxX = items.Max(a => a.X);
        var minY = items.Min(a => a.Y);
        var maxY = items.Max(a => a.Y);

        return new(new(minX, minY), new(maxX, maxY));
    }
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
