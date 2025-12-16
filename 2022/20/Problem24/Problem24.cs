namespace A2022.Problem24;

public static class Solver
{
    [GeneratedTest<int>(18, 230)]
    public static int RunA(string[] lines)
    {
        var (map, startPos, finishPos) = CreateMap(lines);

        var finish1 = CubicPathFinder.CountSteps(map, startPos, finishPos);

        return finish1.Z;
    }

    [GeneratedTest<int>(54, 713)]
    public static int RunB(string[] lines)
    {
        var (map, startPos, finishPos) = CreateMap(lines);

        var finish1 = CubicPathFinder.CountSteps(map, startPos, finishPos);
        var finish2 = CubicPathFinder.CountSteps(map, finish1, startPos);
        var finish3 = CubicPathFinder.CountSteps(map, finish2, finishPos);

        return finish3.Z;
    }

    static (Map3d, Pos3, Pos3) CreateMap(string[] lines)
    {
        var start = FindEmpty(lines[0]);
        var finish = FindEmpty(lines[^1]);

        var map = Simulator.Create3dMap(lines, 2000);

        foreach (var z in map.Size.Z)
        {
            map[start, 0, z] = false;
            map[finish, map.Size.Y - 1, z] = false;
        }

        var startPos = new Pos3(start, 0, 0);
        var finishPos = new Pos3(finish, map.Size.Y - 1, -1);

        return (map, startPos, finishPos);
    }

    static int FindEmpty(string text)
        => text.IndexOf('.');
}
