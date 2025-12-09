namespace Advent.Common;

public readonly record struct Line(Pos Start, Pos End)
{
    public readonly bool IsVertical => Start.X == End.X;

    public readonly Pos Min => Pos.Min(Start, End);
    public readonly Pos Max => Pos.Max(Start, End);
}
