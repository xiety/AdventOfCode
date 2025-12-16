using Advent.Common;

namespace A2022.Problem13;

static class Loader
{
    public static Pair[] Load(string[] lines)
        => lines.SplitBy(String.Empty)
            .ToArray(a =>
            {
                var array = a.ToArray();

                var first = (ItemArray)ParseLine(array[0]);
                var second = (ItemArray)ParseLine(array[^1]);

                return new Pair(first, second);
            });

    public static IEnumerable<ItemArray> LoadItems(string[] lines)
        => lines
            .Where(a => !String.IsNullOrEmpty(a))
            .Select(ParseLine)
            .Cast<ItemArray>();

    public static Item ParseLine(string line)
    {
        if (!line.StartsWith('['))
            return new ItemValue(int.Parse(line));

        var parts = Split(line);

        return new ItemArray(parts.ToArray(ParseLine));
    }

    static IEnumerable<string> Split(string line)
    {
        var deep = 0;
        var current = "";

        foreach (var i in 1..(line.Length - 1))
        {
            var c = line[i];

            if (deep == 0 && c == ',')
            {
                yield return current;
                current = "";
            }
            else
            {
                current += c;

                if (c == '[')
                    deep++;
                else if (c == ']')
                    deep--;
            }
        }

        if (current != "")
            yield return current;
    }
}
