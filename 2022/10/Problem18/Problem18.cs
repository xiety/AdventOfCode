namespace A2022.Problem18;

public static class Solver
{
    [GeneratedTest<int>(64, 3494)]
    public static int RunA(string[] lines)
        => Run(lines, false);

    [GeneratedTest<int>(58, 2062)]
    public static int RunB(string[] lines)
        => Run(lines, true);

    static int Run(string[] lines, bool fill)
    {
        var squares = LoadData(lines);

        var cube = ConstructCube(squares);

        if (fill)
            cube.FillCaverns();

        var items =
            from square in squares
            from offset in Cube.Offsets
            let d = square + offset
            where !cube.IsInside(d) || !cube.HasSquare(d)
            select 1;

        return items.Count();
    }

    static Cube ConstructCube(Pos3[] squares)
    {
        var cube = new Cube(squares.Max(a => a.X) + 1, squares.Max(a => a.Y) + 1, squares.Max(a => a.Z) + 1);

        foreach (var square in squares)
            cube.AddSquare(square);

        return cube;
    }

    static Pos3[] LoadData(string[] lines)
        => lines
            .Select(a => a.Split(",").ToArray(int.Parse))
            .ToArray(a => new Pos3(a[0], a[1], a[2]));
}

class Cube(int width, int height, int depth)
{
    public static readonly Pos3[] Offsets = [
        new(0, 0, +1),
        new(0, 0, -1),
        new(0, +1, 0),
        new(0, -1, 0),
        new(+1, 0, 0),
        new(-1, 0, 0),
    ];

    Pos3 Size { get; } = new(width, height, depth);

    readonly bool[,,] data = new bool[width, height, depth];

    public void AddSquare(Pos3 square)
        => data[square.X, square.Y, square.Z] = true;

    public bool IsInside(Pos3 square)
        => square.X >= 0 && square.X < Size.X
        && square.Y >= 0 && square.Y < Size.Y
        && square.Z >= 0 && square.Z < Size.Z;

    public bool HasSquare(Pos3 square)
        => data[square.X, square.Y, square.Z];

    public void FillCaverns()
    {
        var water = new bool[Size.X, Size.Y, Size.Z];
        var points = new List<Pos3> { new(0, 0, 0) };

        do
        {
            var copy = points.ToArray();

            points.Clear();

            foreach (var point in copy)
            {
                water[point.X, point.Y, point.Z] = true;

                points = points.Union(Offsets
                    .Select(a => a + point)
                    .Where(a => IsInside(a) && !HasSquare(a) && !water[a.X, a.Y, a.Z]))
                    .ToList();
            }
        }
        while (points.Count > 0);

        foreach (var x in Size.X)
            foreach (var y in Size.Y)
                foreach (var z in Size.Z)
                    if (!water[x, y, z])
                        data[x, y, z] = true;
    }
}

