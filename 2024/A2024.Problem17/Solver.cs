using Advent.Common;

namespace A2024.Problem17;

public class Solver : ISolver<string>
{
    public string RunA(string[] lines, bool isSample)
    {
        var (a, b, c, ops) = LoadData(lines);

        var state = new State { A = a, B = b, C = c };
        var output = Run(ops, state);

        return output.StringJoin(",");
    }

    static IEnumerable<int> Run(int[] ops, State state)
    {
        var pointer = 0;

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
        }
        while (pointer < ops.Length - 1);
    }

    static int Out(State state, int p1)
        => Mod8(Combo(state, p1));

    static int Mod8(int n)
        => n & 7;

    static int Div(ref int result, State state, int p1, int pointer)
    {
        result = state.A / (int)Math.Pow(2, Combo(state, p1));
        return pointer + 2;
    }

    static int Adv(State state, int p1, int pointer)
        => Div(ref state.A, state, p1, pointer);

    static int Bdv(State state, int p1, int pointer)
        => Div(ref state.B, state, p1, pointer);

    static int Cdv(State state, int p1, int pointer)
        => Div(ref state.C, state, p1, pointer);

    static int Bxl(State state, int p1, int pointer)
    {
        state.B ^= p1;
        return pointer + 2;
    }

    static int Bst(State state, int p1, int pointer)
    {
        state.B = Combo(state, p1) & 7;
        return pointer + 2;
    }

    static int Jnz(State state, int p1, int pointer)
        => state.A != 0 ? p1 : pointer + 2;

    static int Bxc(State state, int pointer)
    {
        state.B ^= state.C;
        return pointer + 2;
    }

    static int Combo(State state, int n)
        => n switch
        {
            <= 3 => n,
            4 => state.A,
            5 => state.B,
            6 => state.C,
        };

    static (int a, int b, int c, int[] items) LoadData(string[] lines)
    {
        var a = int.Parse(lines[0][(lines[0].LastIndexOf(' ') + 1)..]);
        var b = int.Parse(lines[1][(lines[1].LastIndexOf(' ') + 1)..]);
        var c = int.Parse(lines[2][(lines[2].LastIndexOf(' ') + 1)..]);

        var text = lines[4][(lines[4].LastIndexOf(' ') + 1)..];

        var ops = text.Split(',').Select(int.Parse).ToArray();

        return (a, b, c, ops);
    }
}

class State
{
    public int A;
    public int B;
    public int C;
}
