using Advent.Common;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace A2019.Tests;

[TestClass]
public class AllTests : BaseSolverTest
{
    [ProblemData<int>(33583 + 654 + 2 + 2, 3311492, 50346 + 966 + 2, 4964376)]
    public void Problem01(ITestParameter p) => Test(new Problem01.Solver(), p);

    [ProblemData<int>(3500, 3058646, 0, 8976)]
    public void Problem02(ITestParameter p) => Test(new Problem02.Solver(), p);

    [ProblemData<int>(135, 627, 410, 13190)]
    public void Problem03(ITestParameter p) => Test(new Problem03.Solver(), p);

    [ProblemData<int>(64, 960, 51, 626)]
    public void Problem04(ITestParameter p) => Test(new Problem04.Solver(), p);

    [ProblemData<int>(1, 16489636, 999, 9386583)]
    public void Problem05(ITestParameter p) => Test(new Problem05.Solver(), p);

    [ProblemData<int>(42, 241064, 4, 418)]
    public void Problem06(ITestParameter p) => Test(new Problem06.Solver(), p);

    [ProblemData<int>(65210, 99376, 139629729, 8754464)]
    public void Problem07(ITestParameter p) => Test(new Problem07.Solver(), p);

    [ProblemData<int, string>(4, 1206, Data.Result08Sample, Data.Result08Input)]
    public void Problem08(ITestParameter p) => Test(new Problem08.Solver(), p);
}

public class Data
{
    public const string Result08Sample = """
        .#
        #.
        """;

    public const string Result08Input = """
        ####...##.###...##..###..
        #.......#.#..#.#..#.#..#.
        ###.....#.#..#.#....#..#.
        #.......#.###..#.##.###..
        #....#..#.#.#..#..#.#....
        ####..##..#..#..###.#....
        """;
}
