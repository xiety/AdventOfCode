using Advent.Common;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace A2019.Tests;

[TestClass]
public class AllTests : BaseProblemTest
{
    [ProblemData<int>(33583 + 654 + 2 + 2, 3311492, 50346 + 966 + 2, 4964376)]
    public void Problem01(object p) => Test(new Problem01.Solver(), p);

    [ProblemData<int>(3500, 3058646, 0, 8976)]
    public void Problem02(object p) => Test(new Problem02.Solver(), p);

    [ProblemData<int>(135, 627, 410, 13190)]
    public void Problem03(object p) => Test(new Problem03.Solver(), p);

    [ProblemData<int>(64, 960, 51, 626)]
    public void Problem04(object p) => Test(new Problem04.Solver(), p);

    [ProblemData<int>(1, 16489636, 999, 9386583)]
    public void Problem05(object p) => Test(new Problem05.Solver(), p);
}
