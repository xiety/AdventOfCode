namespace Advent.Common;

public readonly record struct Line(Pos Start, Pos End)
{
    public bool IsVertical => Start.X == End.X;
    public bool IsHorizontal => Start.Y == End.Y;

    public Pos Min => Pos.Min(Start, End);
    public Pos Max => Pos.Max(Start, End);
}
