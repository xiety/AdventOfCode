namespace A2015.Problem11;

public static class Solver
{
    [GeneratedTest<string>("ghjaabcc", "hxbxxyzz")]
    public static string RunA(string[] lines)
        => FindNextGoodPassword(lines[0]);

    [GeneratedTest<string>("ghjbbcdd", "hxcaabcc")]
    public static string RunB(string[] lines)
        => FindNextGoodPassword(FindNextGoodPassword(lines[0]));

    static string FindNextGoodPassword(string text)
    {
        do text = NextPassword(text);
        while (!GoodPassword(text));

        return text;
    }

    static bool GoodPassword(string text)
        => HasNoBadLetters(text) && HasStraight(text) && HasDoubles(text);

    static bool HasNoBadLetters(string s)
        => !s.Any(IsBadLetter);

    static bool IsBadLetter(char c)
        => c is 'i' or 'o' or 'l';

    static bool HasStraight(string text)
        => Enumerable.Range(0, text.Length - 2)
            .Any(i => text[i] + 1 == text[i + 1] && text[i] + 2 == text[i + 2]);

    static bool HasDoubles(string text)
    {
        var n = 0;

        for (var i = 0; i < text.Length - 1; ++i)
        {
            if (text[i] == text[i + 1])
            {
                if (++n == 2)
                    return true;

                i++;
            }
        }

        return false;
    }

    static string NextPassword(string text)
    {
        var chars = text.ToCharArray();

        for (var i = chars.Length - 1; i >= 0; --i)
        {
            var c = chars[i];

            if (c != 'z')
            {
                chars[i]++;

                if (IsBadLetter(chars[i]))
                    chars[i]++;

                break;
            }

            chars[i] = 'a';
        }

        return new(chars);
    }
}
