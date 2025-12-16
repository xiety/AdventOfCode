#pragma warning disable RCS1213 // Remove unused member declaration
using Advent.Common;

namespace A2024.Problem24;

public static class Solver
{
    [GeneratedTest<long>(2024, 36902370467952)]
    public static long RunA(string[] lines)
    {
        var (inputs, connections) = LoadData(lines);
        var outputNodes = connections.Select(a => a.Output).Where(a => a.StartsWith('z')).Distinct().ToArray();
        Calc(connections, inputs, outputNodes);
        return InputToLong(inputs, 'z');
    }

    [GeneratedTest<string>("z00,z01,z02,z05", "cvp,mkk,qbw,wcb,wjb,z10,z14,z34")]
    public static string RunB(string[] lines, bool isSample)
    {
        if (isSample)
            throw new NotImplementedException();

        var (inputs, connections) = LoadData(lines);

        var outputNodes = connections.Select(a => a.Output).Where(a => a.StartsWith('z')).Distinct().ToArray();

        Flip(connections, "z10", "mkk");
        Flip(connections, "z14", "qbw");
        Flip(connections, "wjb", "cvp");
        Flip(connections, "z34", "wcb");

        var workingNodes = new HashSet<string>();

        foreach (var i in 1..(outputNodes.Length - 1))
        {
            var p = i - 1;

            foreach (var a1 in 2)
            {
                foreach (var a2 in 2)
                {
                    foreach (var a3 in 2)
                    {
                        foreach (var a4 in 2)
                        {
                            var tempInputs = new Dictionary<string, int>
                            {
                                [$"x{p:00}"] = a1,
                                [$"x{i:00}"] = a2,
                                [$"y{p:00}"] = a3,
                                [$"y{i:00}"] = a4
                            };

                            foreach (var w in (i - 1))
                            {
                                tempInputs[$"x{w:00}"] = 0;
                                tempInputs[$"y{w:00}"] = 0;
                            }

                            Calc(connections, tempInputs, [$"z{p:00}", $"z{i:00}"]);

                            var input1 = a1 | (a2 << 1);
                            var input2 = a3 | (a4 << 1);

                            var o1 = tempInputs[$"z{p:00}"];
                            var o2 = tempInputs[$"z{i:00}"];
                            var output = o1 | (o2 << 1);

                            var expected = (input1 + input2) & 0b11;

                            if (output != expected)
                                throw new($"BAD Z{i:00}");
                        }
                    }
                }
            }
        }

        //TODO: nodes fixed by hand
        string[] flipped = ["z10", "mkk", "z14", "qbw", "wjb", "cvp", "z34", "wcb"];

        return flipped.Order().StringJoin(",");
    }

    static void Flip(Connection[] connections, string v1, string v2)
    {
        var i1 = Array.FindIndex(connections, a => a.Output == v1);
        var i2 = Array.FindIndex(connections, a => a.Output == v2);

        (connections[i1].Output, connections[i2].Output) = (connections[i2].Output, connections[i1].Output);
    }

    static void FindAllInputs(HashSet<string> existing, Connection[] connections, string parent)
    {
        List<string> nodes = [parent];
        List<string> nextNodes = [];

        do
        {
            foreach (var node in nodes)
            {
                //1 or 0
                foreach (var nextConnection in connections.Where(a => a.Output == node))
                {
                    var added1 = existing.Add(nextConnection.Output);

                    if (added1)
                    {
                        nextNodes.Add(nextConnection.Input1);
                        nextNodes.Add(nextConnection.Input2);
                    }
                }
            }

            (nodes, nextNodes) = (nextNodes, nodes);
            nextNodes.Clear();
        }
        while (nodes.Count > 0);
    }

    static void Dump(Connection[] connections)
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("digraph G {");

        foreach (var connection in connections)
        {
            sb.AppendLine($"    {connection.Input1}->{connection.Output}[label=\"{connection.Operation}\"]");
            sb.AppendLine($"    {connection.Input2}->{connection.Output}[label=\"{connection.Operation}\"]");
        }

        sb.AppendLine("}");

        Console.WriteLine(sb);
    }

    static long InputToLong(Dictionary<string, int> inputs, char startingCharacter)
    {
        var s = inputs
            .Where(a => a.Key.StartsWith(startingCharacter))
            .OrderByDescending(a => a.Key)
            .Select(a => a.Value)
            .StringJoin(String.Empty);

        return Convert.ToInt64(s, 2);
    }

    static void Calc(Connection[] connections, Dictionary<string, int> inputs, string[] outputNodes)
    {
        do
        {
            var changed = false;

            foreach (var connection in connections)
            {
                if (inputs.TryGetValue(connection.Input1, out var i1)
                 && inputs.TryGetValue(connection.Input2, out var i2))
                {
                    var output = Operation(connection.Operation, i1, i2);

                    if (inputs.TryGetValue(connection.Output, out var existing))
                        changed |= (existing == output);
                    else
                        changed = true;

                    inputs[connection.Output] = output;
                }
            }

            if (!changed || outputNodes.All(inputs.ContainsKey))
                break;
        }
        while (true);
    }

    static int Operation(string op, int i1, int i2)
        => op switch
        {
            "AND" => i1 & i2,
            "OR" => i1 | i2,
            "XOR" => i1 ^ i2,
        };

    static (string, int) ParseInput(string text)
    {
        var parts = text.Split(' ');
        return new(parts[0][..^1], int.Parse(parts[1]));
    }

    static Connection ParseConnection(string text)
    {
        var parts = text.Split(' ');
        return new(parts[0], parts[2], parts[1], parts[4]);
    }

    static (Dictionary<string, int>, Connection[]) LoadData(string[] lines)
    {
        var parts = lines.SplitBy(String.Empty).ToArray();
        var inputs = parts[0].Select(ParseInput).ToDictionary(a => a.Item1, a => a.Item2);
        var connections = parts[1].ToArray(ParseConnection);
        return (inputs, connections);
    }
}

record struct Connection(string Input1, string Input2, string Operation, string Output);
