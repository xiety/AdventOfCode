using Advent.Common;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace A2021.Tests;

[TestClass]
public class AllTests : BaseProblemTest
{
    [ProblemTest<int>(7, 1298, 5, 1248)]
    public void Problem01(string filename, bool first, int value)
        => Test(2021, 1, new Problem01.Solver(), filename, first, value);

    [ProblemTest<int>(150, 2322630, 900, 2105273490)]
    public void Problem02(string filename, bool first, int value)
        => Test(2021, 2, new Problem02.Solver(), filename, first, value);

    [ProblemTest<int>(198, 3374136, 230, 4432698)]
    public void Problem03(string filename, bool first, int value)
        => Test(2021, 3, new Problem03.Solver(), filename, first, value);

    [ProblemTest<int>(4512, 49860, 1924, 24628)]
    public void Problem04(string filename, bool first, int value)
        => Test(2021, 4, new Problem04.Solver(), filename, first, value);

    [ProblemTest<int>(5, 6856, 12, 20666)]
    public void Problem05(string filename, bool first, int value)
        => Test(2021, 5, new Problem05.Solver(), filename, first, value);

    [ProblemTest<long>(5934, 391888, 26984457539, 1754597645339)]
    public void Problem06(string filename, bool first, long value)
        => Test(2021, 6, new Problem06.Solver(), filename, first, value);

    [ProblemTest<long>(37, 340052, 168, 92948968)]
    public void Problem07(string filename, bool first, long value)
        => Test(2021, 7, new Problem07.Solver(), filename, first, value);

    [ProblemTest<long>(26, 237, 61229, 1009098)]
    public void Problem08(string filename, bool first, long value)
        => Test(2021, 8, new Problem08.Solver(), filename, first, value);

    [ProblemTest<long>(15, 577, 1134, 1069200)]
    public void Problem09(string filename, bool first, long value)
        => Test(2021, 9, new Problem09.Solver(), filename, first, value);

    [ProblemTest<long>(26397, 278475, 288957, 3015539998)]
    public void Problem10(string filename, bool first, long value)
        => Test(2021, 10, new Problem10.Solver(), filename, first, value);

    [ProblemTest<long>(1656, 1613, 195, 510)]
    public void Problem11(string filename, bool first, long value)
        => Test(2021, 11, new Problem11.Solver(), filename, first, value);

    [ProblemTest<long>(226, 5228, 3509, 131228)]
    public void Problem12(string filename, bool first, long value)
        => Test(2021, 12, new Problem12.Solver(), filename, first, value);

    [ProblemTest<long, string>(17, 693, Data.Result13A, Data.Result13B)]
    public void Problem13(string filename, bool first, long valueA, string valueB)
        => Test(2021, 13, new Problem13.Solver(), filename, first, valueA, valueB);

    [ProblemTest<long>(1588, 3118, 2188189693529, 4332887448171)]
    public void Problem14(string filename, bool first, long value)
        => Test(2021, 14, new Problem14.Solver(), filename, first, value);

    [ProblemTest<long>(40, 415, 315, 2864)]
    public void Problem15(string filename, bool first, long value)
        => Test(2021, 15, new Problem15.Solver(), filename, first, value);

    [ProblemTest<long>(31, 843, 1, 5390807940351)]
    public void Problem16(string filename, bool first, long value)
        => Test(2021, 16, new Problem16.Solver(), filename, first, value);

    [ProblemTest<long>(45, 17766, 112, 1733)]
    public void Problem17(string filename, bool first, long value)
        => Test(2021, 17, new Problem17.Solver(), filename, first, value);

    [ProblemTest<long>(4140, 3524, 3993, 4656)]
    public void Problem18(string filename, bool first, long value)
        => Test(2021, 18, new Problem18.Solver(), filename, first, value);

    [ProblemTest<long>(79, 408, 3621, 13348)]
    public void Problem19(string filename, bool first, long value)
        => Test(2021, 19, new Problem19.Solver(), filename, first, value);

    [ProblemTest<long>(35, 5479, 3351, 19012)]
    public void Problem20(string filename, bool first, long value)
        => Test(2021, 20, new Problem20.Solver(), filename, first, value);

    [ProblemTest<long>(739785, 432450, 444356092776315, 138508043837521)]
    public void Problem21(string filename, bool first, long value)
        => Test(2021, 21, new Problem21.Solver(), filename, first, value);

    [ProblemTest<long>(590784, 533863, 2758514936282235, 1261885414840992)]
    public void Problem22(string filename, bool first, long value)
        => Test(2021, 22, new Problem22.Solver(), filename, first, value);

    [ProblemTest<long>(12521, 15338, 44169, -1)]
    public void Problem23(string filename, bool first, long value)
        => Test(2021, 23, new Problem23.Solver(), filename, first, value);

    [ProblemTest<long>(13579246899999, 0)]
    public void Problem24(string filename, bool first, long value)
        => Test(2021, 24, new Problem24.Solver(), filename, first, value);
}

class Data
{
    public const string Result13A = """
        #####
        #...#
        #...#
        #...#
        #####
        .....
        .....
        """;

    public const string Result13B = """
        #..#..##..#....####.###...##..####.#..#.
        #..#.#..#.#.......#.#..#.#..#....#.#..#.
        #..#.#....#......#..#..#.#..#...#..#..#.
        #..#.#....#.....#...###..####..#...#..#.
        #..#.#..#.#....#....#.#..#..#.#....#..#.
        .##...##..####.####.#..#.#..#.####..##..
        """;
}
