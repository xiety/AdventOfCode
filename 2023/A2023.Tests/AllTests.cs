using Advent.Common;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace A2023.Tests;

[TestClass]
public class AllTests : BaseProblemTest
{
    [ProblemTest<int>(142, 54644, 281, 53348)]
    public void Problem01(string filename, bool first, int value)
        => Test(2023, 1, new Problem01.Solver(), filename, first, value);

    [ProblemTest<int>(8, 3059, 2286, 65371)]
    public void Problem02(string filename, bool first, int value)
        => Test(2023, 2, new Problem02.Solver(), filename, first, value);

    [ProblemTest<int>(4361, 539713, 467835, 84159075)]
    public void Problem03(string filename, bool first, int value)
        => Test(2023, 3, new Problem03.Solver(), filename, first, value);

    [ProblemTest<int>(13, 26426, 30, 6227972)]
    public void Problem04(string filename, bool first, int value)
        => Test(2023, 4, new Problem04.Solver(), filename, first, value);

    [ProblemTest<long>(35, 57075758, 46, 31161857)]
    public void Problem05(string filename, bool first, long value)
        => Test(2023, 5, new Problem05.Solver(), filename, first, value);

    [ProblemTest<long>(288, 1084752, 71503, 28228952)]
    public void Problem06(string filename, bool first, long value)
        => Test(2023, 6, new Problem06.Solver(), filename, first, value);

    [ProblemTest<long>(6440, 249204891, 5905, 249666369)]
    public void Problem07(string filename, bool first, long value)
        => Test(2023, 7, new Problem07.Solver(), filename, first, value);

    [ProblemTest<long>(6, 14893, 0, 0)]
    public void Problem08(string filename, bool first, long value)
        => Test(2023, 8, new Problem08.Solver(), filename, first, value);
}
