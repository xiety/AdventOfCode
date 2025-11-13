using Advent.Common;

namespace A2020.Tests;

[TestClass]
public class AllTests : FolderSolverTest
{
    [ProblemData<int>(514579, 997899, 241861950, 131248694)]
    public void Problem01(ITestParameter p) => Test(new Problem01.Solver(), p);

    [ProblemData<int>(2, 638, 1, 699)]
    public void Problem02(ITestParameter p) => Test(new Problem02.Solver(), p);
}
