using Advent.Common;

namespace A2021.Problem08;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var items = LoadFile(filename);
        var result = items.Sum(a => a.Output.Count(b => b.Length is 2 or 3 or 4 or 7));
        return result;
    }

    public long RunB(string filename)
    {
        var items = LoadFile(filename);
        var result = items.Sum(Analyze);
        return result;
    }

    static List<Item> LoadFile(string filename)
        => File.ReadAllLines(filename)
               .Select(a => a.Split("|", StringSplitOptions.RemoveEmptyEntries))
               .Select(a => new Item(ToArray(a[0]), ToArray(a[1])))
               .ToList();

    static int[][] ToArray(string text)
        => text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(ConvertToNums).ToArray();

    static int[] ConvertToNums(string text)
        => text.Select(a => a - 'a').ToArray();

    static int Analyze(Item item)
    {
        var positions = FindPositions(item);
        var digits = CreateDigits(positions);
        var output = Convert(item.Output, digits);

        var ret = output
            .Reverse()
            .Aggregate((sum: 0, mul: 1), (acc, a) => (acc.sum + a * acc.mul, acc.mul * 10), acc => acc.sum);

        return ret;
    }

    static int[][] CreateDigits(int[] positions)
    {
        int[][] enums = [
            [0, 1, 2, 4, 5, 6],
            [2, 5],
            [0, 2, 3, 4, 6],
            [0, 2, 3, 5, 6],
            [1, 2, 3, 5],
            [0, 1, 3, 5, 6],
            [0, 1, 3, 4, 5, 6],
            [0, 2, 5],
            [0, 1, 2, 3, 4, 5, 6],
            [0, 1, 2, 3, 5, 6],
        ];

        return enums.Select(a => a.Select(b => positions[b]).Order().ToArray()).ToArray();
    }

    static int[] Convert(int[][] output, int[][] digits)
        => output.Select(a => new { Index = Array.FindIndex(digits, b => b.SequenceEqual(a.Order().ToArray())), Value = a })
                 .Select(a => a.Index == -1 ? throw new($"Not found {String.Join(", ", a.Value)} ({a.Value.Length})") : a.Index)
                 .ToArray();

    static int[] FindPositions(Item item)
    {
        /*
         0000
        1    2
        1    2
         3333
        4    5
        4    5
         6666
        */

        var one = item.Input.First(a => a.Length == 2);
        var four = item.Input.First(a => a.Length == 4);

        var times = item.Input
            .SelectMany(a => a)
            .GroupBy(a => a)
            .Select(a => new { Position = a.Key, Count = a.Count() })
            .ToArray();

        var times02 = times.Where(a => a.Count == 8).Select(a => a.Position).ToArray(); //0 or 2
        var times36 = times.Where(a => a.Count == 7).Select(a => a.Position).ToArray(); //3 or 6

        var left = new int[8];

        left[1] = times.First(a => a.Count == 6).Position;
        left[4] = times.First(a => a.Count == 4).Position;
        left[5] = times.First(a => a.Count == 9).Position;

        //1 of 2,5
        left[2] = one.First(a => a != left[5]);
        left[0] = times02.First(a => a != left[2]);

        //4 of 1,2,3,5
        left[3] = four.Contains(times36[0]) ? times36[0] : times36[1];
        left[6] = times36.First(a => a != left[3]);

        return left;
    }
}

record Item(int[][] Input, int[][] Output);
