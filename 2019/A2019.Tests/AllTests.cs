using Advent.Common;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace A2019.Tests;

[TestClass]
public class AllTests : BaseProblemTest
{
    [ProblemData<int>(33583 + 654 + 2 + 2, 3311492, 50346 + 966 + 2, 4964376)]
    public void Problem01(object p)
        => Test(new Problem01.Solver(), p);

    [ProblemData<int>(3500, 3058646, 0, 8976)]
    public void Problem02(object p)
        => Test(new Problem02.Solver(), p);
}
