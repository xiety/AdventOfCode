namespace Advent.Common;

public static class StringExtensions
{
    extension(string text)
    {
        public string TrimLength(int n)
        {
            if (text.Length <= n)
                return text;

            const string ellipsis = "...";

            text = text[..(n - ellipsis.Length)];

            return text + ellipsis;
        }
    }
}
