using Advent.Common;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace A2019.Tests;

[TestClass]
public class AllTests : BaseProblemTest
{
    [ProblemData<int>(33583 + 654 + 2 + 2, 3311492, 50346 + 966 + 2, 4964376)]
    public void Problem01(bool isA, bool isSample, int value)
        => Test(2019, 1, new Problem01.Solver(), isA, isSample, value);

    [ProblemData<int>(3500, 3058646, 0, 8976)]
    public void Problem02(bool isA, bool isSample, int value)
        => Test(2019, 2, new Problem02.Solver(), isA, isSample, value);
}
