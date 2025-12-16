namespace A2021.Problem18;

public static class Solver
{
    [GeneratedTest<long>(4140, 3524)]
    public static long RunA(string[] lines)
    {
        var items = lines.ToArray(Parse);

        var result = items.Skip(1).Aggregate(items[0], AddNodes);

        var tree = ToTree(result.Tokens.ToArray());

        return Magnitude(tree);
    }

    [GeneratedTest<long>(3993, 4656)]
    public static long RunB(string[] lines)
    {
        var items = lines.ToArray(Parse);

        return items
            .SelectMany(a => items
                .Where(b => b != a)
                .Select(b =>
                {
                    var result = AddNodes(a, b);
                    var tree = ToTree(result.Tokens.ToArray());
                    return Magnitude(tree);
                }))
            .Max();
    }

    static int Magnitude(Tree tree)
        => tree switch
        {
            TreeBranch tb => Magnitude(tb.Left) * 3 + Magnitude(tb.Right) * 2,
            TreeLeaf tv => tv.Value,
        };

    static Tree ToTree(ReadOnlySpan<Token> input)
    {
        switch (input[0])
        {
            case TokenOpenBranch:
                var inner = input[1..^1];

                var n = inner[0] is TokenOpenBranch
                    ? FindClosing(inner, 1)
                    : inner.IndexOf(new TokenComma());

                var left = ToTree(inner[0..n]);
                var right = ToTree(inner[(n + 1)..]);

                return new TreeBranch(left, right);

            case TokenValue tv:
                return new TreeLeaf(tv.Value);

            default:
                throw new();
        }
    }

    static int FindClosing(ReadOnlySpan<Token> inner, int startingIndex)
    {
        var depth = 1;

        foreach (var index in startingIndex..inner.Length)
        {
            if (inner[index] == new TokenOpenBranch())
            {
                depth++;
            }
            else if (inner[index] == new TokenCloseBranch())
            {
                depth--;

                if (depth == 0)
                    return index + 1;
            }
        }

        throw new();
    }

    static Node AddNodes(Node left, Node right)
    {
        IEnumerable<Token> Internal()
        {
            yield return new TokenOpenBranch();

            foreach (var token in left.Tokens)
                yield return token;

            yield return new TokenComma();

            foreach (var token in right.Tokens)
                yield return token;

            yield return new TokenCloseBranch();
        }

        var result = new Node(Internal().ToList());

        Reduce(result);

        return result;
    }

    static void Reduce(Node input)
    {
        var repeat = false;

        do
        {
            repeat = Explode(input);

            if (!repeat)
                repeat = Split(input);
        }
        while (repeat);
    }

    static bool Split(Node input)
    {
        foreach (var i in input.Tokens.Count)
        {
            var token = input.Tokens[i];

            if (token is TokenValue tv && tv.Value >= 10)
            {
                var nd = tv.Value / 2.0;

                var left = (int)Math.Floor(nd);
                var right = (int)Math.Ceiling(nd);

                input.Tokens.RemoveAt(i);

                input.Tokens.InsertRange(i, [
                    new TokenOpenBranch(),
                    new TokenValue(left),
                    new TokenComma(),
                    new TokenValue(right),
                    new TokenCloseBranch(),
                ]);

                return true;
            }
        }

        return false;
    }

    static bool Explode(Node input)
    {
        var depth = 0;

        foreach (var i in input.Tokens.Count)
        {
            var token = input.Tokens[i];

            if (token is TokenOpenBranch)
            {
                depth++;
            }
            else if (token is TokenCloseBranch)
            {
                depth--;
            }
            else
            {
                if (depth >= 5)
                {
                    var leftValue = ((TokenValue)token).Value;
                    var rightValue = ((TokenValue)input.Tokens[i + 2]).Value;

                    input.Tokens.RemoveRange(i - 1, 5);
                    input.Tokens.Insert(i - 1, new TokenValue(0));

                    for (var j = i - 2; j >= 0; --j)
                    {
                        if (input.Tokens[j] is TokenValue tv)
                        {
                            input.Tokens[j] = new TokenValue(tv.Value + leftValue);
                            break;
                        }
                    }

                    foreach (var j in (i + 1)..input.Tokens.Count)
                    {
                        if (input.Tokens[j] is TokenValue tv)
                        {
                            input.Tokens[j] = new TokenValue(tv.Value + rightValue);
                            break;
                        }
                    }

                    return true;
                }
            }
        }

        return false;
    }

    static Node Parse(string text)
    {
        return new(Internal().ToList());

        IEnumerable<Token> Internal()
        {
            foreach (var i in text.Length)
            {
                switch (text[i])
                {
                    case '[':
                        yield return new TokenOpenBranch();
                        break;
                    case ']':
                        yield return new TokenCloseBranch();
                        break;
                    case ',':
                        yield return new TokenComma();
                        break;
                    default:
                        {
                            var j = i;

                            for (; j < text.Length; ++j)
                            {
                                if (!Char.IsDigit(text[j]))
                                    break;
                            }

                            yield return new TokenValue(int.Parse(text[i..j]));
                            break;
                        }
                }
            }
        }
    }
}

record Node(List<Token> Tokens)
{
    public override string ToString()
        => Tokens.Select(TokenToString).StringJoin();

    static string TokenToString(Token t)
        => t switch
        {
            TokenOpenBranch => "[",
            TokenCloseBranch => "]",
            TokenComma => ",",
            TokenValue tv => $"{tv.Value}",
        };
}

abstract record Token;
record TokenOpenBranch : Token;
record TokenCloseBranch : Token;
record TokenComma : Token;
record TokenValue(int Value) : Token;

abstract record Tree;
record TreeBranch(Tree Left, Tree Right) : Tree;
record TreeLeaf(int Value) : Tree;
