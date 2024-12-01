using Advent.Common;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace A2024.Tests;

[TestClass]
public class AllTests : BaseSolverTest
{
    [ProblemData<int>(11, 1151792, 31, 21790168)]
    public void Problem01(ITestParameter p) => Test(new Problem01.Solver(), p);
}
