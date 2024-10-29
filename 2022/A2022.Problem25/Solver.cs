using Advent.Common;

namespace A2022.Problem25;

public class Solver : IProblemSolver<string, bool>
{
    public string RunA(string filename)
    {
        var data = File.ReadAllLines(filename);

        checked
        {
            var sum = 0L;

            foreach (var item in data)
            {
                var dec = SnafuConverter.ToDecimal(item);
                sum += dec;
            }

            var result = SnafuConverter.ToSnafu(sum);

            return result;
        }
    }

    public bool RunB(string filename)
        => true;
}
