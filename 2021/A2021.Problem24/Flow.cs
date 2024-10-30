namespace A2021.Problem24;

public class Flow
{
    const string input_str = "00000000000000";

    public string Run(string filename)
    {
        var lines = File.ReadAllLines(filename);

        var data = new List<Item>[4];

        for (var i = 0; i < data.Length; ++i)
            data[i] = [new(0, input_str)];

        var linenum = 1;
        var input_index = 1;

        foreach (var line in lines)
        {
            var (op, rest) = GetOp(line);

            switch (op)
            {
                case "inp":
                    var index = GetIndex(rest);
                    data[index].Clear();

                    data[index].AddRange(
                        Enumerable.Range(1, 9)
                        .Select(a => new Item(a, input_str[..(input_index - 1)] + a + input_str[(input_index)..]))
                        .ToArray());

                    input_index++;
                    break;
                case "add" or "mul" or "div" or "mod" or "eql":
                    var (a, b) = GetParts(rest);
                    var ia = GetIndex(a);
                    var ib = GetIndex(b);

                    Item[] right;

                    if (ib >= 0)
                        right = [.. data[ib]];
                    else
                        right = [new Item(int.Parse(b), input_str)];

                    var result = new List<Item>();

                    foreach (var pa in data[ia])
                    {
                        foreach (var pb in right)
                        {
                            if (op == "mod" && (pa.Value < 0 || pb.Value <= 0))
                                continue;

                            if (op == "div" && pb.Value == 0)
                                continue;

                            if (!CanMerge(pa.Origins, pb.Origins))
                                continue;

                            var merged = Merge(pa.Origins, pb.Origins);

                            if (op == "mul" && pb.Value == 0)
                                merged = pb.Origins;

                            var result_value = op switch
                            {
                                "add" => pa.Value + pb.Value,
                                "mul" => pa.Value * pb.Value,
                                "div" => pa.Value / pb.Value,
                                "mod" => pa.Value % pb.Value,
                                "eql" => pa.Value == pb.Value ? 1 : 0
                            };

                            result.Add(new(result_value, merged));
                        }
                    }

                    //data[ia] = [.. result.GroupBy(a => a.Value).Select(a => a.MaxBy(b => b.Origins))];
                    data[ia] = [.. result.Distinct()];

                    break;
            }

            Console.WriteLine($"{linenum}: {line} | {String.Join(" | ", data.Select(b => b.Count))}");
            linenum++;
        }

        var ret = data[2].Where(a => a.Value == 0).MaxBy(a => a. Origins);

        return ret.Origins;
    }

    private bool CanMerge(string a, string b)
    {
        for (var i = 0; i < a.Length; ++i)
        {
            if (a[i] != '0' && b[i] != '0' && a[i] != b[i])
                return false;
        }

        return true;
    }

    private string Merge(string a, string b)
    {
        var r = "";

        for (var i = 0; i < a.Length; ++i)
        {
            r += a[i] > b[i] ? a[i] : b[i];
        }

        return r;
    }

    private static int GetIndex(string text)
        => text switch
        {
            "x" => 0,
            "y" => 1,
            "z" => 2,
            "w" => 3,
            _ => -1,
        };

    private (string, string) GetParts(string rest)
    {
        var n = rest.IndexOf(' ');
        return (rest[..n], rest[(n + 1)..]);
    }

    private (string, string) GetOp(string line)
    {
        var n = line.IndexOf(' ');
        return (line[..n], line[(n + 1)..]);
    }
}

record struct Item(long Value, string Origins);
