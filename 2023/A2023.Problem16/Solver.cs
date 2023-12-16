using Advent.Common;

namespace A2023.Problem16;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var map = MapData.ParseMap(File.ReadAllLines(filename), c => c);
        var energy = new int[map.GetWidth(), map.GetHeight()];

        Go(map, energy);

        return energy.Enumerate().Count(pair => pair.item > 0);
    }

    private void Go(char[,] map, int[,] energy)
    {
        var rays = new List<MutableRay>() { new(new(-1, 0), new(1, 0), []) };

        var step = 0;

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
                        if (ray.Path.Contains((newPos, ray.Dir)))
                        {
                            ray.Stopped = true;
                        }
                        else
                        {
                            ray.Path.Add((newPos, ray.Dir));

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

            step++;
        }
        while (rays.Any(a => !a.Stopped));
    }
}

public record class MutableRay
{
    public Pos Pos { get; set; }
    public Pos Dir { get; set; }

    public bool Stopped { get; set; }

    public List<(Pos, Pos)> Path { get; }

    public MutableRay(Pos pos, Pos dir, List<(Pos, Pos)> path)
        => (Pos, Dir, Path) = (pos, dir, path);
}
