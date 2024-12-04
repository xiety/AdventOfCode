using Advent.Common;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace A2024.Tests;

[TestClass]
public class AllTests : BaseSolverTest
{
    [ProblemData<int>(11, 1151792, 31, 21790168)]
    public void Problem01(ITestParameter p) => Test(new Problem01.Solver(), p);

    [ProblemData<int>(2, 213, 4, 285)]
    public void Problem02(ITestParameter p) => Test(new Problem02.Solver(), p);

    [ProblemData<int>(161, 170068701, 48, 78683433)]
    public void Problem03(ITestParameter p) => Test(new Problem03.Solver(), p);

    [ProblemData<int>(18, 2397, 9, 1824)]
    public void Problem04(ITestParameter p) => Test(new Problem04.Solver(), p);
}
