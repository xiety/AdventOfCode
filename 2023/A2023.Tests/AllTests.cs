using Advent.Common;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace A2023.Tests;

[TestClass]
public class AllTests : BaseProblemTest
{
    [ProblemTest<int>(142, 54644, 281, 53348)]
    public void Problem01(string filename, bool first, int value)
        => Test(2023, 1, new Problem01.Solver(), filename, first, value);

    [ProblemTest<int>(8, 3059, 2286, 65371)]
    public void Problem02(string filename, bool first, int value)
        => Test(2023, 2, new Problem02.Solver(), filename, first, value);

    [ProblemTest<int>(4361, 539713, 467835, 84159075)]
    public void Problem03(string filename, bool first, int value)
        => Test(2023, 3, new Problem03.Solver(), filename, first, value);

    [ProblemTest<int>(13, 26426, 30, 6227972)]
    public void Problem04(string filename, bool first, int value)
        => Test(2023, 4, new Problem04.Solver(), filename, first, value);

    [ProblemTest<long>(35, 57075758, 46, 31161857)]
    public void Problem05(string filename, bool first, long value)
        => Test(2023, 5, new Problem05.Solver(), filename, first, value);

    [ProblemTest<long>(288, 1084752, 71503, 28228952)]
    public void Problem06(string filename, bool first, long value)
        => Test(2023, 6, new Problem06.Solver(), filename, first, value);

    [ProblemTest<long>(6440, 249204891, 5905, 249666369)]
    public void Problem07(string filename, bool first, long value)
        => Test(2023, 7, new Problem07.Solver(), filename, first, value);

    [ProblemTest<long>(6, 14893, 6, 10241191004509)]
    public void Problem08(string filename, bool first, long value)
        => Test(2023, 8, new Problem08.Solver(), filename, first, value);

    [ProblemTest<long>(114, 1972648895, 2, 919)]
    public void Problem09(string filename, bool first, long value)
        => Test(2023, 9, new Problem09.Solver(), filename, first, value);

    [ProblemTest<long>(8, 7005, 10, 417)]
    public void Problem10(string filename, bool first, long value)
        => Test(2023, 10, new Problem10.Solver(), filename, first, value);

    [ProblemTest<long>(374, 9233514, 8410, 363293506944)]
    public void Problem11(string filename, bool first, long value)
        => Test(2023, 11, new Problem11.Solver(), filename, first, value);

    [ProblemTest<long>(21, 7460, 525152, 6720660274964)]
    public void Problem12(string filename, bool first, long value)
        => Test(2023, 12, new Problem12.Solver(), filename, first, value);

    [ProblemTest<long>(405, 30158, 400, 36474)]
    public void Problem13(string filename, bool first, long value)
        => Test(2023, 13, new Problem13.Solver(), filename, first, value);

    [ProblemTest<long>(136, 109654, 64, 94876)]
    public void Problem14(string filename, bool first, long value)
        => Test(2023, 14, new Problem14.Solver(), filename, first, value);

    [ProblemTest<long>(1320, 505459, 145, 228508)]
    public void Problem15(string filename, bool first, long value)
        => Test(2023, 15, new Problem15.Solver(), filename, first, value);

    [ProblemTest<long>(46, 7307, 51, 7635)]
    public void Problem16(string filename, bool first, long value)
        => Test(2023, 16, new Problem16.Solver(), filename, first, value);

    [ProblemTest<long>(102, 928, 94, 1104)]
    public void Problem17(string filename, bool first, long value)
        => Test(2023, 17, new Problem17.Solver(), filename, first, value);

    [ProblemTest<long>(62, 106459, 952408144115, 63806916814808)]
    public void Problem18(string filename, bool first, long value)
        => Test(2023, 18, new Problem18.Solver(), filename, first, value);

    [ProblemTest<long>(19114, 350678, 167409079868000, 124831893423809)]
    public void Problem19(string filename, bool first, long value)
        => Test(2023, 19, new Problem19.Solver(), filename, first, value);

    [ProblemTest<long>(11687500, 747304011, 1, 220366255099387)]
    public void Problem20(string filename, bool first, long value)
        => Test(2023, 20, new Problem20.Solver(), filename, first, value);

    [ProblemTest<long>(16, 3724, 0, 0)]
    public void Problem21(string filename, bool first, long value)
        => Test(2023, 21, new Problem21.Solver(), filename, first, value);

    [ProblemTest<long>(5, 405, 7, 61297)]
    public void Problem22(string filename, bool first, long value)
        => Test(2023, 22, new Problem22.Solver(), filename, first, value);

    [ProblemTest<long>(94, 2218, 0, 0)]
    public void Problem23(string filename, bool first, long value)
        => Test(2023, 23, new Problem23.Solver(), filename, first, value);

    [ProblemTest<long>(2, 11098, 47, 0)]
    public void Problem24(string filename, bool first, long value)
        => Test(2023, 24, new Problem24.Solver(), filename, first, value);

    [ProblemTest<long>(54, 596376)]
    public void Problem25(string filename, bool first, long value)
        => Test(2023, 25, new Problem25.Solver(), filename, first, value);
}
