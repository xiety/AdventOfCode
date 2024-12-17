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

    [ProblemData<int>(143, 4872, 123, 5564)]
    public void Problem05(ITestParameter p) => Test(new Problem05.Solver(), p);

    [ProblemData<int>(41, 5404, 6, 1984)]
    public void Problem06(ITestParameter p) => Test(new Problem06.Solver(), p);

    [ProblemData<long>(3749, 882304362421, 11387, 145149066755184)]
    public void Problem07(ITestParameter p) => Test(new Problem07.Solver(), p);

    [ProblemData<int>(14, 240, 34, 955)]
    public void Problem08(ITestParameter p) => Test(new Problem08.Solver(), p);

    [ProblemData<long>(1928, 6330095022244, 2858, 6359491814941)]
    public void Problem09(ITestParameter p) => Test(new Problem09.Solver(), p);

    [ProblemData<int>(36, 798, 81, 1816)]
    public void Problem10(ITestParameter p) => Test(new Problem10.Solver(), p);

    [ProblemData<long>(55312, 209412, 65601038650482, 248967696501656)]
    public void Problem11(ITestParameter p) => Test(new Problem11.Solver(), p);

    [ProblemData<long>(1930, 1352976, 1206, 808796)]
    public void Problem12(ITestParameter p) => Test(new Problem12.Solver(), p);

    [ProblemData<long>(480, 29522, 875318608908, 101214869433312)]
    public void Problem13(ITestParameter p) => Test(new Problem13.Solver(), p);

    [ProblemData<long>(12, 236628054, -1, 7584)]
    public void Problem14(ITestParameter p) => Test(new Problem14.Solver(), p);

    [ProblemData<long>(10092, 1471826, 9021, 1457703)]
    public void Problem15(ITestParameter p) => Test(new Problem15.Solver(), p);

    [ProblemData<long>(7036, 83432, -1, -1)]
    public void Problem16(ITestParameter p) => Test(new Problem16.Solver(), p);

    [ProblemData<string>("4,6,3,5,6,3,5,2,1,0", "1,6,7,4,3,0,5,0,6", "", "")]
    public void Problem17(ITestParameter p) => Test(new Problem17.Solver(), p);
}
