using Advent.Common;

namespace A2024.Problem21;

public static class Solver
{
    [GeneratedTest<long>(126384, 215374)]
    public static long RunA(string[] lines)
        => Run(lines, 2);

    [GeneratedTest<long>(154115708116294, 260586897262600)]
    public static long RunB(string[] lines)
        => Run(lines, 25);

    static long Run(string[] lines, int num)
        => lines.Sum(a => long.Parse(a[..^1]) * Find(a, num));

    static long Find(string sequence, int num)
        => Memoization.RunRecursive<string, int, long>(sequence, 0,
            (memo, seq, level) => num == level - 1
                ? seq.Length
                : FindOnMap(level == 0 ? mapPad : mapJoy, seq)
                    .Min(a => a.Sum(b => memo(b, level + 1))));

    static string[][] FindOnMap(char[,] panel, string sequence)
    {
        var map = panel.ToArray(a => a switch { ' ' => -1, _ => 1 });

        (string[][] Paths, Pos Start) init = ([[]], panel.FindValue('A'));

        return sequence.Prepend('A').Chain()
            .Aggregate(init, (acc, item) =>
            {
                if (item.First != item.Second)
                {
                    var end = panel.FindValue(item.Second);

                    var paths = PathFinder.FindAll(map, acc.Start, end)
                        .SelectMany(path => acc.Paths
                            .ToArray(a => (string[])[.. a, ToMovements(path.Prepend(acc.Start))]))
                        .ToArray();

                    return (paths, end);
                }
                else
                {
                    var paths = acc.Paths.ToArray(a => (string[])[.. a, "A"]);
                    return (paths, acc.Start);
                }
            }).Paths;
    }

    static string ToMovements(IEnumerable<Pos> path)
        => new([.. path.Chain().Select(a => a.Second - a.First).Select(MapToButton), 'A']);

    static char MapToButton(Pos dir)
        => dir switch
        {
            { X: 0, Y: -1 } => '^',
            { X: 1, Y: 0 } => '>',
            { X: 0, Y: 1 } => 'v',
            { X: -1, Y: 0 } => '<',
        };

    static readonly char[,] mapPad = {
        { '7', '8', '9' },
        { '4', '5', '6' },
        { '1', '2', '3' },
        { ' ', '0', 'A' },
    };

    static readonly char[,] mapJoy = {
        { ' ', '^', 'A' },
        { '<', 'v', '>' },
    };

    static Solver()
    {
        mapPad = mapPad.Transposed();
        mapJoy = mapJoy.Transposed();
    }
}
