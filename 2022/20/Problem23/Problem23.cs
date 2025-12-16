using Advent.Common;

namespace A2022.Problem23;

public static class Solver
{
    [GeneratedTest<int>(110, 3862)]
    public static int RunA(string[] lines)
        => Run(lines, 10);

    [GeneratedTest<int>(20, 913)]
    public static int RunB(string[] lines)
        => Run(lines, -1);

    static int Run(string[] data, int stop)
    {
        var dwarfOffsets = new Dictionary<Pos, Pos>();
        var newPositions = new Dictionary<Pos, int>();

        var dwarfs = ParseMap(data);
        var step = 0;

        do
        {
            var moved = FirstStep(dwarfs, dwarfOffsets, newPositions, step % 4);

            if (moved == 0)
                break;

            SecondStep(dwarfOffsets, newPositions);

            ThirdStep(dwarfs, dwarfOffsets);

            step++;

            if (step == stop)
                break;

            dwarfOffsets.Clear();
            newPositions.Clear();
        }
        while (true);

        if (stop != -1)
            return CountEmpty(dwarfs);

        return step + 1;
    }

    static int CountEmpty(HashSet<Pos> dwarfs)
    {
        var min = GetMin(dwarfs);
        var max = GetMax(dwarfs);

        return (max.X - min.X + 1) * (max.Y - min.Y + 1) - dwarfs.Count;
    }

    static Pos GetMin(IReadOnlyCollection<Pos> dwarfs)
    {
        var minX = dwarfs.Min(a => a.X);
        var minY = dwarfs.Min(a => a.Y);

        return new(minX, minY);
    }

    static Pos GetMax(IReadOnlyCollection<Pos> dwarfs)
    {
        var maxX = dwarfs.Max(a => a.X);
        var maxY = dwarfs.Max(a => a.Y);

        return new(maxX, maxY);
    }

    static void ThirdStep(HashSet<Pos> dwarfs, Dictionary<Pos, Pos> dwarfOffsets)
    {
        dwarfs.Clear();

        foreach (var dwarfOffsetPair in dwarfOffsets)
            dwarfs.Add(dwarfOffsetPair.Value);
    }

    static int FirstStep(HashSet<Pos> dwarfs, Dictionary<Pos, Pos> dwarfOffsets, Dictionary<Pos, int> newPositions, int start)
    {
        var moved = 0;

        foreach (var dwarf in dwarfs)
        {
            var offset = -1;
            var notBusy = 0;

            foreach (var i in 4)
            {
                var busy = false;

                var index = (start + i) % 4;

                foreach (var j in 3)
                {
                    if (dwarfs.Contains(offsets[index, j] + dwarf))
                    {
                        busy = true;
                        break;
                    }
                }

                if (!busy)
                {
                    if (offset == -1)
                        offset = index;

                    notBusy++;
                }
                else
                {
                    if (offset >= 0)
                        break;
                }
            }

            if (notBusy == 4)
                offset = -1;

            if (offset >= 0)
            {
                var newPos = offsets[offset, 0] + dwarf;

                dwarfOffsets.Add(dwarf, newPos);
                newPositions.AddOrReplace(newPos, 1, a => a + 1);

                moved++;
            }
            else
            {
                dwarfOffsets.Add(dwarf, dwarf);
            }
        }

        return moved;
    }

    static void SecondStep(Dictionary<Pos, Pos> dwarfOffsets, Dictionary<Pos, int> newPositions)
    {
        foreach (var newPositionPair in newPositions)
            if (newPositionPair.Value > 1)
                foreach (var dwarfOffsetPair in dwarfOffsets)
                    if (dwarfOffsetPair.Value == newPositionPair.Key)
                        dwarfOffsets[dwarfOffsetPair.Key] = dwarfOffsetPair.Key;
    }

    static HashSet<Pos> ParseMap(string[] data)
    {
        var dwarfs = new HashSet<Pos>();

        foreach (var y in data.Length)
            foreach (var x in data[y].Length)
                if (data[y][x] == '#')
                    dwarfs.Add(new(x, y));

        return dwarfs;
    }

    static readonly Pos[,] offsets = new Pos[,]
    {
        { new(0,-1), new(+1,-1), new(-1,-1) }, //N, NE, NW
        { new(0,+1), new(+1,+1), new(-1,+1) }, //S, SE, SW
        { new(-1,0), new(-1,-1), new(-1,+1) }, //W, NW, SW
        { new(+1,0), new(+1,-1), new(+1,+1) }, //E, NE, SE
    };
}
