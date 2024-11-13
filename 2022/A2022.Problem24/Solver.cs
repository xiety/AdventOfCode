using Advent.Common;

namespace A2022.Problem24;

public class Solver : IProblemSolver<int>
{
    public int RunA(string filename)
    {
        var (map, startPos, finishPos) = CreateMap(filename);

        var finish1 = CubicPathFinder.CountSteps(map, startPos, finishPos);

        return finish1.Z;
    }

    public int RunB(string filename)
    {
        var (map, startPos, finishPos) = CreateMap(filename);

        var finish1 = CubicPathFinder.CountSteps(map, startPos, finishPos);
        var finish2 = CubicPathFinder.CountSteps(map, finish1, startPos);
        var finish3 = CubicPathFinder.CountSteps(map, finish2, finishPos);

        return finish3.Z;
    }

    static (Map3d, Pos3, Pos3) CreateMap(string filename)
    {
        var data = File.ReadAllLines(filename);

        var start = FindEmpty(data[0]);
        var finish = FindEmpty(data[^1]);

        var map = Simulator.Create3dMap(data, 2000);

        for (var z = 0; z < map.Size.Z; ++z)
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
