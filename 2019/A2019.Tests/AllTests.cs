using Advent.Common;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace A2019.Tests;

[TestClass]
public class AllTests : BaseProblemTest
{
    [ProblemTest<int>(33583 + 654 + 2 + 2, 3311492, 50346 + 966 + 2, 4964376)]
    public void Problem01(string filename, bool first, int value)
        => Test(2019, 1, new Problem01.Solver(), filename, first, value);

    [ProblemTest<int>(3500, 3058646, 0, 8976)]
    public void Problem02(string filename, bool first, int value)
        => Test(2019, 2, new Problem02.Solver(), filename, first, value);
}
