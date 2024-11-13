using Advent.Common;

namespace A2023.Problem15;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
    {
        var items = File.ReadAllText(filename).TrimEnd().Split(",");

        return items.Sum(Hash);
    }

    public long RunB(string filename)
    {
        var items = File.ReadAllText(filename).TrimEnd().Split(",").ToArray();

        var boxes = ArrayEx.CreateAndInitialize(256, _ => new List<(string label, int focal)>());

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

        //.Reverse<(string label, int focal)>()
        return boxes.Select((list, index) =>
            list.Select((b, index2) => (index + 1) * (index2 + 1) * b.focal).Sum()).Sum();
    }

    long Hash(string text)
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
