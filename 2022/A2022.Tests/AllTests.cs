using Advent.Common;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace A2022.Tests;

[TestClass]
public class AllTests : BaseProblemTest
{
    [ProblemTest<int>(24000, 71506, 45000, 209603)]
    public void Problem01(string filename, bool first, int value)
        => Test(2022, 1, new Problem01.Solver(), filename, first, value);

    [ProblemTest<int>(15, 12772, 12, 11618)]
    public void Problem02(string filename, bool first, int value)
        => Test(2022, 2, new Problem02.Solver(), filename, first, value);

    [ProblemTest<int>(157, 7863, 70, 2488)]
    public void Problem03(string filename, bool first, int value)
        => Test(2022, 3, new Problem03.Solver(), filename, first, value);
}
