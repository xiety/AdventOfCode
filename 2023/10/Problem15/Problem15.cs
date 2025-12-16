namespace A2023.Problem15;

public static class Solver
{
    [GeneratedTest<long>(1320, 505459)]
    public static long RunA(string[] lines)
    {
        var items = lines[0].TrimEnd().Split(",");
        return items.Sum(Hash);
    }

    [GeneratedTest<long>(145, 228508)]
    public static long RunB(string[] lines)
    {
        var items = lines[0].TrimEnd().Split(",").ToArray();

        var boxes = Array.CreateAndInitialize(256, _ => new List<(string label, int focal)>());

        foreach (var item in items)
        {
            var n = item.IndexOf('-');

            if (n >= 0)
            {
                var label = item[..n];
                var boxNum = Hash(label);
                var box = boxes[boxNum];

                box.RemoveAll(a => a.label == label);
            }
            else
            {
                n = item.IndexOf('=');
                var label = item[..n];

                var num = int.Parse(item[(n + 1)..]);

                var boxNum = Hash(label);
                var box = boxes[boxNum];

                var p = box.FindIndex(a => a.label == label);

                if (p >= 0)
                {
                    box[p] = (label, num);
                }
                else
                {
                    box.Add((label, num));
                }
            }
        }

        return boxes.Select((list, index) =>
            list.Select((b, index2) => (index + 1) * (index2 + 1) * b.focal).Sum()).Sum();
    }

    static long Hash(string text)
    {
        var ret = 0;

        var bytes = System.Text.Encoding.ASCII.GetBytes(text);

        foreach (var b in bytes)
        {
            ret += b;
            ret *= 17;
            ret %= 256;
        }

        return ret;
    }
}
