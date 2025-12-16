using Advent.Common;

namespace A2022.Problem25;

public static class Solver
{
    [GeneratedTest<string>("2=-1=0", "20===-20-020=0001-02")]
    public static string RunA(string[] lines)
    {
        checked
        {
            var sum = 0L;

            foreach (var item in lines)
            {
                var dec = SnafuConverter.ToDecimal(item);
                sum += dec;
            }

            return SnafuConverter.ToSnafu(sum);
        }
    }
}
