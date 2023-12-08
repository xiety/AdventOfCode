using System.Text.RegularExpressions;

using Advent.Common;

namespace A2021.Problem17;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
        => Run(filename).Item1;

    public long RunB(string filename)
        => Run(filename).Item2;

    private (long, long) Run(string filename)
    {
        var item = CompiledRegs.MapRegex().FromFile<Item>(filename).First();

        var globalMaxY = Int32.MinValue;

        var startVY = item.FromY;

        var hitCount = 0;

        do
        {
            var startVX = 0;

            do
            {
                var x = 0;
                var y = 0;

                var vx = startVX;
                var vy = startVY;

                var maxY = Int32.MinValue;
                var hit = false;

                do
                {
                    x += vx;
                    y += vy;

                    if (maxY < y)
                        maxY = y;

                    if (vx > 0)
                        vx--;
                    else if (vx < 0)
                        vx++;

                    vy--;

                    if (x >= item.FromX && x <= item.ToX && y >= item.FromY && y <= item.ToY)
                    {
                        hit = true;
                        break;
                    }

                    if (x > item.ToX || y < item.FromY)
                        break;
                }
                while (true);

                if (hit)
                {
                    hitCount++;

                    if (globalMaxY < maxY)
                        globalMaxY = maxY;
                }

                startVX++;

                if (startVX > item.ToX)
                    break;
            }
            while (true);

            startVY++;

            if (startVY > 200) //cheat
                break;
        }
        while (true);

        return (globalMaxY, hitCount);
    }
}

public record Item(int FromX, int FromY, int ToX, int ToY);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^target area: x=(?<{nameof(Item.FromX)}>-?\d+)\.\.(?<{nameof(Item.ToX)}>-?\d+), y=(?<{nameof(Item.FromY)}>-?\d+)\.\.(?<{nameof(Item.ToY)}>-?\d+)$")]
    public static partial Regex MapRegex();
}
