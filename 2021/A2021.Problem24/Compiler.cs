using System.Text;

namespace A2021.Problem24;

public static class Compiler
{
    public static string Run(string filename)
    {
        var lines = File.ReadAllLines(filename);

        return Compile(lines);
    }

    static string Compile(string[] lines)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"public int Run(long input)");
        sb.AppendLine($"{{");
        sb.AppendLine($"    int w = 0, x = 0, y = 0, z = 0;");
        sb.AppendLine($"    var input_index = 10_000_000_000_000L;");
        sb.AppendLine();

        foreach (var line in lines)
        {
            var (op, rest) = GetOp(line);

            switch (op)
            {
                case "inp":
                    {
                        sb.AppendLine($"    {rest} = (int)(input % input_index);");
                        sb.AppendLine($"    input_index /= 10;");
                        break;
                    }
                case "add":
                    {
                        var (name1, name2) = GetParts(rest);
                        sb.AppendLine($"    {name1} += {name2};");
                        break;
                    }
                case "mul":
                    {
                        var (name1, name2) = GetParts(rest);
                        sb.AppendLine($"    {name1} *= {name2};");
                        break;
                    }
                case "div":
                    {
                        var (name1, name2) = GetParts(rest);
                        sb.AppendLine($"    if ({name2} == 0) return -1;");
                        sb.AppendLine($"    {name1} /= {name2};");
                        break;
                    }
                case "mod":
                    {
                        var (name1, name2) = GetParts(rest);
                        sb.AppendLine($"    if ({name1} < 0 || {name2} <= 0) return -1;");
                        sb.AppendLine($"    {name1} %= {name2};");
                        break;
                    }
                case "eql":
                    {
                        var (name1, name2) = GetParts(rest);
                        sb.AppendLine($"    {name1} = ({name1} == {name2}) ? 1 : 0;");
                        break;
                    }
            }
        }

        sb.AppendLine($"    return z;");
        sb.AppendLine($"}}");

        return sb.ToString();
    }

    static (string, string) GetParts(string rest)
    {
        var n = rest.IndexOf(' ');
        return (rest[..n], rest[(n + 1)..]);
    }

    static (string, string) GetOp(string line)
    {
        var n = line.IndexOf(' ');
        return (line[..n], line[(n + 1)..]);
    }
}
