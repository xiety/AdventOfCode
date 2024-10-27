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

    [ProblemTest<long>(13, 6339, 36, 2541)]
    public void Problem09(string filename, bool first, long value)
        => Test(2022, 9, new Problem09.Solver(), filename, first, value);

    [ProblemTest<int, string>(13140, 13820, Data.Result09A, Data.Result09B)]
    public void Problem10(string filename, bool first, int valueA, string valueB)
        => Test(2022, 10, new Problem10.Solver(), filename, first, valueA, valueB);

    [ProblemTest<long>(10605, 50616, 2713310158, 11309046332)]
    public void Problem11(string filename, bool first, long value)
        => Test(2022, 11, new Problem11.Solver(), filename, first, value);

    [ProblemTest<int>(31, 456, 29, 454)]
    public void Problem12(string filename, bool first, int value)
        => Test(2022, 12, new Problem12.Solver(), filename, first, value);

    [ProblemTest<int>(13, 6478, 140, 21922)]
    public void Problem13(string filename, bool first, int value)
        => Test(2022, 13, new Problem13.Solver(), filename, first, value);

    [ProblemTest<int>(24, 768, 93, 26686)]
    public void Problem14(string filename, bool first, int value)
        => Test(2022, 14, new Problem14.Solver(), filename, first, value);

    [ProblemTest<long>(26, 5335787, 56000011, 13673971349056)]
    public void Problem15(string filename, bool first, long value)
        => Test(2022, 15, new Problem15.Solver(), filename, first, value);

    [ProblemTest<int>(1651, 1617, 1707, 2171)]
    public void Problem16(string filename, bool first, int value)
        => Test(2022, 16, new Problem16.Solver(), filename, first, value);

    [ProblemTest<long>(3068, 3055, 1514285714288, 1507692307690)]
    public void Problem17(string filename, bool first, long value)
        => Test(2022, 17, new Problem17.Solver(), filename, first, value);

    [ProblemTest<int>(64, 3494, 58, 2062)]
    public void Problem18(string filename, bool first, int value)
        => Test(2022, 18, new Problem18.Solver(), filename, first, value);

    [ProblemTest<int>(33, 1958, 62 * 56, 4257)]
    public void Problem19(string filename, bool first, int value)
        => Test(2022, 19, new Problem19.Solver(), filename, first, value);

    [ProblemTest<long>(3, 4224, 1623178306, 861907680486)]
    public void Problem20(string filename, bool first, long value)
        => Test(2022, 20, new Problem20.Solver(), filename, first, value);

    [ProblemTest<long>(152, 81075092088442, 301, 3349136384441)]
    public void Problem21(string filename, bool first, long value)
        => Test(2022, 21, new Problem21.Solver(), filename, first, value);
}

public class Data
{
    public const string Result09A = """
        ##..##..##..##..##..##..##..##..##..##..
        ###...###...###...###...###...###...###.
        ####....####....####....####....####....
        #####.....#####.....#####.....#####.....
        ######......######......######......####
        #######.......#######.......#######.....
        """;

    public const string Result09B = """
        ####.#..#..##..###..#..#..##..###..#..#.
        ...#.#.#..#..#.#..#.#.#..#..#.#..#.#.#..
        ..#..##...#....#..#.##...#....#..#.##...
        .#...#.#..#.##.###..#.#..#.##.###..#.#..
        #....#.#..#..#.#.#..#.#..#..#.#.#..#.#..
        ####.#..#..###.#..#.#..#..###.#..#.#..#.
        """;
}
