using Advent.Common;

namespace A2024.Problem17;

public class Solver : ISolver<string, long>
{
    public string RunA(string[] lines, bool isSample)
    {
        var (a, b, c, ops) = LoadData(lines);
        var state = new State { A = a, B = b, C = c };
        var output = Cpu.Run(ops, state);
        return output.StringJoin(",");
    }

    public long RunB(string[] lines, bool isSample)
    {
        var (a, b, c, ops) = LoadData(lines);

        if (isSample)
        {
            var state = new State();

            var i = 0L;

            do
            {
                state.A = i;
                state.B = b;
                state.C = c;

                var output = Cpu.Run(ops, state);

                if (ops.SequenceEqual(output))
                    return i;

                i++;
            }
            while (true);
        }
        else
        {
            var patternsAll = CreatePatterns();

            var start = "000000000";
            var target = ops.Reverse().ToArray();
            var index = 0;

            var binary = Recurse(patternsAll, target, index, start).Order().First();

            return Convert.ToInt64(binary, 2);
        }
    }

    //TODO: works only with my input
    static string[][] CreatePatterns()
    {
        var list = new List<List<string>>();

        for (var i = 0; i < 8; ++i)
        {
            var sublist = new List<string>();

            for (var j = 0; j < 8; ++j)
            {
                var a1 = i ^ j;
                var a2 = j ^ 0b110;
                var a3 = a2 ^ 0b11;
                var a4 = a1 << a3;
                var a5 = a4 | a2;

                var a1bin = ToBin(a1);
                var text = a1bin + new string('X', a3);
                text = text[0..(text.Length - 3)] + ToBin(a2);

                if (text[..3] == a1bin)
                    sublist.Add(text);
            }

            list.Add(sublist);
        }

        return list.Select(a => a.ToArray()).ToArray();
    }

    static string ToBin(int n)
        => Convert.ToString(n, 2).PadLeft(3, '0');

    static (long a, long b, long c, long[] items) LoadData(string[] lines)
    {
        var a = long.Parse(lines[0][(lines[0].LastIndexOf(' ') + 1)..]);
        var b = long.Parse(lines[1][(lines[1].LastIndexOf(' ') + 1)..]);
        var c = long.Parse(lines[2][(lines[2].LastIndexOf(' ') + 1)..]);

        var text = lines[4][(lines[4].LastIndexOf(' ') + 1)..];

        var ops = text.Split(',').Select(long.Parse).ToArray();

        return (a, b, c, ops);
    }

    static IEnumerable<string> Recurse(string[][] patternsAll, long[] target, int index, string start)
    {
        if (index >= target.Length)
        {
            yield return start;
            yield break;
        }

        var patterns = patternsAll[target[index]];

        foreach (var pattern in patterns.Where(a => a != ""))
        {
            var body = pattern[..(pattern.Length - 3)];
            var end = pattern[(pattern.Length - 3)..];

            var found = true;

            for (var i = 0; i < body.Length; ++i)
            {
                var b = body[body.Length - 1 - i];
                var s = start[start.Length - 1 - i];

                if (b != 'X' && b != s)
                {
                    found = false;
                    break;
                }
            }

            if (found)
            {
                foreach (var child in Recurse(patternsAll, target, index + 1, start + end))
                    yield return child;
            }
        }
    }
}

class Cpu
{
    public static IEnumerable<long> Run(long[] ops, State state)
    {
        var pointer = 0;
        var index = 0;

        do
        {
            var op = ops[pointer];
            var p1 = ops[pointer + 1];

            if (op == 5)
            {
                yield return Out(state, p1);
                pointer += 2;
            }
            else
            {
                pointer = op switch
                {
                    0 => Adv(state, p1, pointer),
                    1 => Bxl(state, p1, pointer),
                    2 => Bst(state, p1, pointer),
                    3 => Jnz(state, p1, pointer),
                    4 => Bxc(state, pointer),
                    6 => Bdv(state, p1, pointer),
                    7 => Cdv(state, p1, pointer),
                };
            }

            index++;

            if (index > int.MaxValue)
                break;
        }
        while (pointer < ops.Length - 1);
    }

    static long Out(State state, long p1)
        => Mod8(Combo(state, p1));

    static long Mod8(long n)
        => n & 7;

    static int Div(ref long result, State state, long p1, int pointer)
    {
        result = state.A / (long)Math.Pow(2, Combo(state, p1));
        return pointer + 2;
    }

    static int Adv(State state, long p1, int pointer)
        => Div(ref state.A, state, p1, pointer);

    static int Bdv(State state, long p1, int pointer)
        => Div(ref state.B, state, p1, pointer);

    static int Cdv(State state, long p1, int pointer)
        => Div(ref state.C, state, p1, pointer);

    static int Bxl(State state, long p1, int pointer)
    {
        state.B ^= p1;
        return pointer + 2;
    }

    static int Bst(State state, long p1, int pointer)
    {
        state.B = Mod8(Combo(state, p1));
        return pointer + 2;
    }

    static int Jnz(State state, long p1, int pointer)
        => state.A != 0 ? (int)p1 : pointer + 2;

    static int Bxc(State state, int pointer)
    {
        state.B ^= state.C;
        return pointer + 2;
    }

    static long Combo(State state, long n)
        => n switch
        {
            <= 3 => n,
            4 => state.A,
            5 => state.B,
            6 => state.C,
        };
}

class State
{
    public long A;
    public long B;
    public long C;
}
