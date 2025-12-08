namespace Advent.Common;

public class Cube(Pos3 min, Pos3 max)
{
    public static readonly Pos3[] Offsets = [
        new(0, 0, +1),
        new(0, 0, -1),
        new(0, +1, 0),
        new(0, -1, 0),
        new(+1, 0, 0),
        new(-1, 0, 0),
    ];

    public Pos3 Min => min;
    public Pos3 Max => max;
    public Pos3 Size => max - min;

    public bool IsInside(Pos3 pos)
        => pos.X >= min.X && pos.X < max.X
        && pos.Y >= min.Y && pos.Y < max.Y
        && pos.Z >= min.Z && pos.Z < max.Z;
}
