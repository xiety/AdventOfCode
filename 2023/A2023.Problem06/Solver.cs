using System.Text.RegularExpressions;
using System.Xml;

using Advent.Common;

namespace A2023.Problem06;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var lines = File.ReadAllLines(filename);
        var times = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToArray();
        var distances = lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToArray();

        var result = 1;

        foreach (var (time, distance) in Enumerable.Zip(times, distances))
        {
            Console.WriteLine($"{time} {distance}");
            var win = 0;

            for (var i = 0; i < time; ++i)
            {
                var r = (time - i) * i;

                if (r > distance)
                {
                    Console.WriteLine($"  {i} {r}");
                    win++;
                }
            }

            result *= win;
        }

        return result;
    }
}
