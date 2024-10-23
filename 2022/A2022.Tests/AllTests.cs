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

    [ProblemTest<int>(2, 542, 4, 900)]
    public void Problem04(string filename, bool first, int value)
        => Test(2022, 4, new Problem04.Solver(), filename, first, value);

    [ProblemTest<string>("CMZ", "GRTSWNJHH", "MCD", "QLFQDBBHM")]
    public void Problem05(string filename, bool first, string value)
        => Test(2022, 5, new Problem05.Solver(), filename, first, value);

    [ProblemTest<int>(7, 1892, 19, 2313)]
    public void Problem06(string filename, bool first, int value)
        => Test(2022, 6, new Problem06.Solver(), filename, first, value);

    [ProblemTest<long>(95437, 1427048, 24933642, 2940614)]
    public void Problem07(string filename, bool first, long value)
        => Test(2022, 7, new Problem07.Solver(), filename, first, value);

    [ProblemTest<long>(21, 1845, 8, 230112)]
    public void Problem08(string filename, bool first, long value)
        => Test(2022, 8, new Problem08.Solver(), filename, first, value);
}
