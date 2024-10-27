using System.Text.RegularExpressions;

using Advent.Common;

namespace A2021.Problem13;

public class Solver : IProblemSolver<long, string>
{
    public long RunA(string filename)
    {
        var (map, folds) = LoadFile(filename);

        var folded = Fold(map, folds.First());

        var result = folded.Cast<bool>().Count(a => a);

        return result;
    }

    public string RunB(string filename)
    {
        var (map, folds) = LoadFile(filename);

        var folded = folds.Aggregate(map, Fold);
        
        return folded.ToString(Environment.NewLine, "", a => a ? "#" : ".")
            .TrimEnd();
    }

    private static (bool[,], FoldItem[]) LoadFile(string filename)
    {
        var (firstPart, secondPart) = File.ReadAllLines(filename).Split(String.Empty);

        var map = ParseMap(firstPart);
        var folds = ParseFolds(secondPart);

        return (map, folds);
    }

    private static bool[,] Fold(bool[,] parent, FoldItem fold)
    {
        if (fold.Axis == "x")
        {
            var map = new bool[fold.Num, parent.GetHeight()];

            for (var y = 0; y < parent.GetHeight(); ++y)
            {
                for (var x = 0; x < fold.Num; ++x)
                {
                    map[x, y] = parent[x, y];

                    var side = fold.Num * 2 - x;

                    if (side >= 0 && side < parent.GetWidth())
                        map[x, y] |= parent[side, y];
                }
            }

            return map;
        }
        else
        {
            var map = new bool[parent.GetWidth(), fold.Num];

            for (var x = 0; x < parent.GetWidth(); ++x)
            {
                for (var y = 0; y < fold.Num; ++y)
                {
                    map[x, y] = parent[x, y];

                    var side = fold.Num * 2 - y;

                    if (side >= 0 && side < parent.GetHeight())
                        map[x, y] |= parent[x, side];
                }
            }

            return map;
        }
    }

    private static bool[,] ParseMap(IEnumerable<string> lines)
    {
        var items = CompiledRegs.MapRegex().FromLines<MapItem>(lines);

        var width = items.Max(a => a.X) + 1;
        var height = items.Max(a => a.Y) + 1;

        var map = new bool[width, height];

        items.ForEach(a => map[a.X, a.Y] = true);

        return map;
    }

    private static FoldItem[] ParseFolds(IEnumerable<string> lines)
        => CompiledRegs.FoldRegex().FromLines<FoldItem>(lines).ToArray();
}

public record MapItem(int X, int Y);
public record FoldItem(string Axis, int Num);

static partial class CompiledRegs
{
    [GeneratedRegex(@$"^(?<{nameof(MapItem.X)}>\d+),(?<{nameof(MapItem.Y)}>\d+)$")]
    public static partial Regex MapRegex();

    [GeneratedRegex(@$"^fold along (?<{nameof(FoldItem.Axis)}>\w)=(?<{nameof(FoldItem.Num)}>\d+)")]
    public static partial Regex FoldRegex();
}
