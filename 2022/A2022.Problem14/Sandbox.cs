namespace A2022.Problem14;

class Sandbox(int width, int height)
{
    private readonly UnitType[,] data = new UnitType[width, height];
    private readonly Pos[] downDeltas = [new(0, 1), new(-1, 1), new(1, 1)];

    //mutable
    private Pos currentPos;

    public bool Emit(Pos pos, UnitType unitType)
    {
        if (data.Get(pos) == UnitType.Empty)
        {
            data.Set(pos, unitType);
            currentPos = pos;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddWalls(List<Pos[]> items)
    {
        foreach (var item in items)
            foreach (var (a, b) in item.Chain())
                AddWall(a, b);
    }

    private void AddWall(Pos a, Pos b)
    {
        if (a.X != b.X)
        {
            var min = Math.Min(a.X, b.X);
            var max = Math.Max(a.X, b.X);

            for (var x = min; x <= max; ++x)
                data[x, a.Y] = UnitType.Wall;
        }
        else
        {
            var min = Math.Min(a.Y, b.Y);
            var max = Math.Max(a.Y, b.Y);

            for (var y = min; y <= max; ++y)
                data[a.X, y] = UnitType.Wall;
        }
    }

    public StepResult RunStep()
    {
        var moved = 0;
        var outOfBounds = 0;

        var p = currentPos;

        var tomove = downDeltas
            .Select(a => a + p)
            .Where(a => !data.IsInBounds(a) || data.Get(a) == UnitType.Empty)
            .FirstOrNull();

        if (tomove is Pos t)
        {
            if (data.IsInBounds(t))
                data.Set(t, UnitType.Sand);
            else
                outOfBounds++;

            data.Set(p, UnitType.Empty);
            moved++;

            currentPos = t;
        }
        else
        {
            data.Set(p, UnitType.Fixed);
        }

        return new(moved, outOfBounds);
    }
}

readonly record struct StepResult(int Moved, int OutOfBounds);

enum UnitType
{
    Empty,
    Wall,
    Sand,
    Fixed,
}
