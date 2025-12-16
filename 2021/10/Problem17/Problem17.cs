using System.Text.RegularExpressions;

namespace A2021.Problem17;

public static class Solver
{
    [GeneratedTest<long>(45, 17766)]
    public static long RunA(string[] lines)
        => Run(lines).Item1;

    [GeneratedTest<long>(112, 1733)]
    public static long RunB(string[] lines)
        => Run(lines).Item2;

    static (long, long) Run(string[] lines)
    {
        var item = CompiledRegs.MapToMapRegex(lines[0]);

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
                }
                while (x <= item.ToX && y >= item.FromY);

                if (hit)
                {
                    hitCount++;

                    if (globalMaxY < maxY)
                        globalMaxY = maxY;
                }

                startVX++;
            }
            while (startVX <= item.ToX);

            startVY++;
        }
        while (startVY <= 200); //cheat

        return (globalMaxY, hitCount);
    }
}

record Item(int FromX, int FromY, int ToX, int ToY);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^target area: x=(?<{nameof(Item.FromX)}>-?\d+)\.\.(?<{nameof(Item.ToX)}>-?\d+), y=(?<{nameof(Item.FromY)}>-?\d+)\.\.(?<{nameof(Item.ToY)}>-?\d+)$")]
    [MapTo<Item>]
    public static partial Regex MapRegex();
}
