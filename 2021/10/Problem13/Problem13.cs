using System.Text.RegularExpressions;

namespace A2021.Problem13;

public static class Solver
{
    [GeneratedTest<long>(17, 693)]
    public static long RunA(string[] lines)
    {
        var (map, folds) = LoadData(lines);

        var folded = Fold(map, folds[0]);

        return folded.Cast<bool>().Count(a => a);
    }

    [GeneratedTest<string>(ResultData.Result13A, ResultData.Result13B)]
    public static string RunB(string[] lines)
    {
        var (map, folds) = LoadData(lines);

        var folded = folds.Aggregate(map, Fold);

        return folded.ToDump(Environment.NewLine, "", a => a ? "#" : ".")
            .TrimEnd();
    }

    static bool[,] Fold(bool[,] parent, FoldItem fold)
    {
        if (fold.Axis == "x")
        {
            var map = new bool[fold.Num, parent.Height];

            foreach (var y in parent.Height)
            {
                foreach (var x in fold.Num)
                {
                    map[x, y] = parent[x, y];

                    var side = fold.Num * 2 - x;

                    if (side >= 0 && side < parent.Width)
                        map[x, y] |= parent[side, y];
                }
            }

            return map;
        }
        else
        {
            var map = new bool[parent.Width, fold.Num];

            foreach (var x in parent.Width)
            {
                foreach (var y in fold.Num)
                {
                    map[x, y] = parent[x, y];

                    var side = fold.Num * 2 - y;

                    if (side >= 0 && side < parent.Height)
                        map[x, y] |= parent[x, side];
                }
            }

            return map;
        }
    }

    static bool[,] ParseMap(IEnumerable<string> lines)
    {
        var items = CompiledRegs.FromLinesMapRegex(lines);

        var width = items.Max(a => a.X) + 1;
        var height = items.Max(a => a.Y) + 1;

        var map = new bool[width, height];

        items.ForEach(a => map[a.X, a.Y] = true);

        return map;
    }

    static FoldItem[] ParseFolds(IEnumerable<string> lines)
        => CompiledRegs.FromLinesFoldRegex(lines);

    static (bool[,], FoldItem[]) LoadData(string[] lines)
    {
        var (firstPart, secondPart) = lines.SplitBy(String.Empty);

        var map = ParseMap(firstPart);
        var folds = ParseFolds(secondPart);

        return (map, folds);
    }
}

public record MapItem(int X, int Y);
public record FoldItem(string Axis, int Num);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(MapItem.X)}>\d+),(?<{nameof(MapItem.Y)}>\d+)$")]
    [MapTo<MapItem>]
    public static partial Regex MapRegex();

    [GeneratedRegex(@$"^fold along (?<{nameof(FoldItem.Axis)}>\w)=(?<{nameof(FoldItem.Num)}>\d+)")]
    [MapTo<FoldItem>]
    public static partial Regex FoldRegex();
}

static class ResultData
{
    public const string Result13A = """
        #####
        #...#
        #...#
        #...#
        #####
        .....
        .....
        """;

    public const string Result13B = """
        #..#..##..#....####.###...##..####.#..#.
        #..#.#..#.#.......#.#..#.#..#....#.#..#.
        #..#.#....#......#..#..#.#..#...#..#..#.
        #..#.#....#.....#...###..####..#...#..#.
        #..#.#..#.#....#....#.#..#..#.#....#..#.
        .##...##..####.####.#..#.#..#.####..##..
        """;
}
