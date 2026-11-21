namespace Advent.Common;

public static class StringExtensions
{
    extension(string text)
    {
        public IEnumerable<int> AllIndexesOf(string value, StringComparison comparison = StringComparison.Ordinal)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(value))
                yield break;

            var index = 0;
            while (true)
            {
                var f = text.IndexOf(value, index, comparison);
                if (f == -1)
                    break;

                yield return f;

                index = f + value.Length;
            }
        }

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
