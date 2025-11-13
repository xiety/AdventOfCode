using Advent.Common;

namespace A2023.Problem16;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var map = MapData.ParseMap(File.ReadAllLines(filename), c => c);
        return Calculate(map, new(-1, 0), new(1, 0));
    }

    public long RunB(string filename)
    {
        var map = MapData.ParseMap(File.ReadAllLines(filename), c => c);

        var max = -1;
        var value = 0;

        for (var x = 0; x < map.Width; ++x)
        {
            value = Calculate(map, new(x, -1), new(0, 1));

            if (value > max)
                max = value;

            value = Calculate(map, new(x, map.Height), new(0, -1));

            if (value > max)
                max = value;
        }

        for (var y = 0; y < map.Height; ++y)
        {
            value = Calculate(map, new(-1, y), new(1, 0));

            if (value > max)
                max = value;

            value = Calculate(map, new(map.Width, y), new(-1, 0));

            if (value > max)
                max = value;
        }

        return max;
    }

    static int Calculate(char[,] map, Pos startingPos, Pos startingDir)
    {
        var energy = new int[map.Width, map.Height];
        Go(map, energy, startingPos, startingDir);
        return energy.Enumerate().Count(pair => pair.Item > 0);
    }

    static void Go(char[,] map, int[,] energy, Pos startingPos, Pos startingDir)
    {
        var rays = new List<MutableRay>() { new(startingPos, startingDir, []) };

        do
        {
            foreach (var ray in rays.ToArray())
            {
                if (!ray.Stopped)
                {
                    var newPos = ray.Pos + ray.Dir;

                    if (!map.IsInBounds(newPos))
                    {
                        ray.Stopped = true;
                    }
                    else
                    {
                        if (!ray.Path.Add((newPos, ray.Dir)))
                        {
                            ray.Stopped = true;
                        }
                        else
                        {
                            energy.Set(newPos, energy.Get(newPos) + 1);

                            var c = map.Get(newPos);
                            var newDir = ray.Dir;

                            if (c == '\\')
                            {
                                newDir = new(ray.Dir.Y, ray.Dir.X);
                            }
                            else if (c == '/')
                            {
                                newDir = new(-ray.Dir.Y, -ray.Dir.X);
                            }
                            else if (c == '-')
                            {
                                if (ray.Dir == new Pos(0, 1) || ray.Dir == new Pos(0, -1))
                                {
                                    newDir = new Pos(-1, 0);

                                    var newRay = new MutableRay(newPos, new Pos(1, 0), ray.Path);
                                    rays.Add(newRay);
                                }
                            }
                            else if (c == '|')
                            {
                                if (ray.Dir == new Pos(1, 0) || ray.Dir == new Pos(-1, 0))
                                {
                                    newDir = new Pos(0, -1);

                                    var newRay = new MutableRay(newPos, new Pos(0, 1), ray.Path);
                                    rays.Add(newRay);
                                }
                            }

                            ray.Dir = newDir;
                            ray.Pos = newPos;
                        }
                    }
                }
            }
        }
        while (rays.Any(a => !a.Stopped));
    }
}

public class MutableRay(Pos pos, Pos dir, HashSet<(Pos, Pos)> path)
{
    public Pos Pos { get; set; } = pos;
    public Pos Dir { get; set; } = dir;

    public bool Stopped { get; set; }

    public HashSet<(Pos, Pos)> Path { get; } = path;
}
