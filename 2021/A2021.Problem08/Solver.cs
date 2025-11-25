using Advent.Common;

namespace A2021.Problem08;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
        => LoadFile(filename).Sum(a => a.Output.Count(b => b.Length is 2 or 3 or 4 or 7));

    public long RunB(string filename)
        => LoadFile(filename).Sum(Analyze);

    static int[][] ToArray(string text)
        => text.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray(ConvertToNums);

    static int[] ConvertToNums(string text)
        => text.ToArray(a => a - 'a');

    static int Analyze(Item item)
    {
        var positions = FindPositions(item);
        var digits = CreateDigits(positions);
        var output = Convert(item.Output, digits);

        return output
            .Reverse()
            .Aggregate((sum: 0, mul: 1), (acc, a) => (acc.sum + a * acc.mul, acc.mul * 10), acc => acc.sum);
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

        return enums.ToArray(a => a.Select(b => positions[b]).Order().ToArray());
    }

    static int[] Convert(int[][] output, int[][] digits)
        => output.Select(a => new { Index = Array.FindIndex(digits, b => b.SequenceEqual([.. a.Order()])), Value = a })
                 .ToArray(a => a.Index == -1 ? throw new($"Not found {String.Join(", ", a.Value)} ({a.Value.Length})") : a.Index);

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
            .CountBy(a => a)
            .ToArray();

        var times02 = times.Where(a => a.Value == 8).ToArray(a => a.Key); //0 or 2
        var times36 = times.Where(a => a.Value == 7).ToArray(a => a.Key); //3 or 6

        var left = new int[8];

        left[1] = times.First(a => a.Value == 6).Key;
        left[4] = times.First(a => a.Value == 4).Key;
        left[5] = times.First(a => a.Value == 9).Key;

        //1 of 2,5
        left[2] = one.First(a => a != left[5]);
        left[0] = times02.First(a => a != left[2]);

        //4 of 1,2,3,5
        left[3] = four.Contains(times36[0]) ? times36[0] : times36[1];
        left[6] = times36.First(a => a != left[3]);

        return left;
    }

    static Item[] LoadFile(string filename)
        => File.ReadAllLines(filename)
               .Select(a => a.Split("|", StringSplitOptions.RemoveEmptyEntries))
               .ToArray(a => new Item(ToArray(a[0]), ToArray(a[1])));
}

record Item(int[][] Input, int[][] Output);
