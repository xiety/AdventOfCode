using Advent.Common;

namespace A2020.Tests;

[TestClass]
public class AllTests : FolderSolverTest
{
    [ProblemData<int>(514579, 997899, 241861950, 131248694)]
    public void Problem01(ITestParameter p) => Test(new Problem01.Solver(), p);

    [ProblemData<int>(2, 638, 1, 699)]
    public void Problem02(ITestParameter p) => Test(new Problem02.Solver(), p);

    [ProblemData<long>(7, 187, 336, 4723283400)]
    public void Problem03(ITestParameter p) => Test(new Problem03.Solver(), p);

    [ProblemData<int>(2, 254, 2, 184)]
    public void Problem04(ITestParameter p) => Test(new Problem04.Solver(), p);

    [ProblemData<int>(820, 938, 356, 696)]
    public void Problem05(ITestParameter p) => Test(new Problem05.Solver(), p);

    [ProblemData<int>(11, 6457, 6, 3260)]
    public void Problem06(ITestParameter p) => Test(new Problem06.Solver(), p);
}
