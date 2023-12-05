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
}
