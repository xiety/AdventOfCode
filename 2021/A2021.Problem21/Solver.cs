using Advent.Common;

namespace A2021.Problem21;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var players = LoadFile(filename);

        const int target = 1000;

        var step = 0;
        var scores = new int[players.Length];

        do
        {
            for (var i = 0; i < players.Length; ++i)
            {
                var sum = 0;

                for (var j = 0; j < 3; ++j)
                {
                    sum += ((step + 1) % 100);
                    step++;
                }

                players[i] += sum;
                players[i] = (players[i] - 1) % 10 + 1;

                scores[i] += players[i];

                if (scores[i] >= target)
                    goto end;
            }
        }
        while (true);

    end:
        var result = scores.Min() * step;

        return result;
    }

    public long RunB(string filename)
    {
        var players = LoadFile(filename);

        var dic = new Dictionary<int, int>
        {
            [3] = 1,
            [4] = 3,
            [5] = 6,
            [6] = 7,
            [7] = 6,
            [8] = 3,
            [9] = 1,
        };

        const int target = 21;

        var (win1, win2) = Recurse(0, 0, 0, players[0], players[1], target, dic);

        return Math.Max(win1, win2);
    }

    static (long, long) Recurse(int level, int score1, int score2, int position1, int position2, int target, Dictionary<int, int> dic)
    {
        if (score1 >= target)
            return (1, 0);

        if (score2 >= target)
            return (0, 1);

        var who = (level % 2) == 0;

        var win1 = 0L;
        var win2 = 0L;

        foreach (var pair in dic)
        {
            var p1 = who ? (position1 + pair.Key - 1) % 10 + 1 : position1;
            var p2 = !who ? (position2 + pair.Key - 1) % 10 + 1 : position2;

            var s1 = who ? score1 + p1 : score1;
            var s2 = !who ? score2 + p2 : score2;

            var (w1, w2) = Recurse(level + 1, s1, s2, p1, p2, target, dic);

            win1 += w1 * pair.Value;
            win2 += w2 * pair.Value;
        }

        return (win1, win2);
    }

    static int[] LoadFile(string filename)
    {
        var lines = File.ReadAllLines(filename);
        var p1 = int.Parse(lines[0]["Player 1 starting position: ".Length..]);
        var p2 = int.Parse(lines[1]["Player 2 starting position: ".Length..]);
        return [p1, p2];
    }
}
