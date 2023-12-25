using Advent.Common;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace A2019.Tests;

[TestClass]
public class AllTests : BaseProblemTest
{
    [ProblemTest<int>(34241, 3311492, 0, 0)]
    public void Problem01(string filename, bool first, int value)
        => Test(2019, 1, new Problem01.Solver(), filename, first, value);
}
