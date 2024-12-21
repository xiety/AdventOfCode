namespace A2022.Problem13;

static class Loader
{
    public static IEnumerable<Pair> Load(string filename)
    {
        var lines = File.ReadAllLines(filename);
        var chunks = lines.Split(String.Empty);

        foreach (var chunk in chunks)
        {
            var array = chunk.ToArray();

            var first = (ItemArray)ParseLine(array.First());
            var second = (ItemArray)ParseLine(array.Last());

            yield return new(first, second);
        }
    }

    public static IEnumerable<ItemArray> LoadItems(string filename)
        => File.ReadAllLines(filename)
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

        for (var i = 1; i < line.Length - 1; ++i)
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
