using System.Globalization;

using Advent.Common;

namespace A2021.Problem04;

public class Solver : IProblemSolver<int>
{
    const int sizeX = 5;
    const int sizeY = 5;

    public int RunA(string filename)
    {
        var (numbers, boards) = LoadFile(filename);

        var marks = CreateEmptyMarks(boards);
        var result = -1;

        foreach (var number in numbers)
        {
            for (var i = 0; i < boards.Length; ++i)
            {
                MarkBoardCell(boards[i], marks[i], number);

                var bingo = CheckBingos(marks[i]);

                if (bingo)
                {
                    result = CalculateResult(boards[i], marks[i], number);
                    goto end;
                }
            }
        }

    end:
        return result;
    }

    public int RunB(string filename)
    {
        var (numbers, boards) = LoadFile(filename);

        var marks = CreateEmptyMarks(boards);
        var wins = new bool[boards.Length];
        var result = -1;

        foreach (var number in numbers)
        {
            for (var i = 0; i < boards.Length; ++i)
            {
                MarkBoardCell(boards[i], marks[i], number);

                var bingo = CheckBingos(marks[i]);

                if (bingo)
                {
                    wins[i] = true;

                    if (wins.All(a => a == true))
                    {
                        result = CalculateResult(boards[i], marks[i], number);
                        goto end;
                    }
                }
            }
        }

    end:
        return result;
    }

    private static bool[][,] CreateEmptyMarks(int[][,] boards)
    {
        var marks = new bool[boards.Length][,];

        for (var i = 0; i < marks.Length; ++i)
            marks[i] = new bool[sizeX, sizeY];

        return marks;
    }

    private static void MarkBoardCell(int[,] boards, bool[,] marks, int number)
    {
        for (var y = 0; y < sizeY; ++y)
            for (var x = 0; x < sizeX; ++x)
                if (boards[x, y] == number)
                    marks[x, y] = true;
    }

    private static (int[] numbers, int[][,]) LoadFile(string filename)
    {
        var items = File.ReadAllLines(filename);
        var numbers = items[0].Split(',').Select(int.Parse).ToArray();
        var boards = ParseBoards(items[2..]);

        return (numbers, boards);
    }

    private static int[][,] ParseBoards(string[] items)
    {
        var list = new List<int[,]>();

        foreach (var part in items.Split(String.Empty).Select(a => a.ToArray()))
        {
            var array = new int[sizeX, sizeY];

            for (var y = 0; y < sizeY; ++y)
            {
                var line = part[y]
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse);

                array.SetRow(y, line);
            }

            list.Add(array);
        }

        return list.ToArray();
    }

    private static int CalculateResult(int[,] board, bool[,] mark, int number)
    {
        var sum = 0;

        for (var y = 0; y < sizeY; ++y)
            for (var x = 0; x < sizeX; ++x)
                if (!mark[x, y])
                    sum += board[x, y];

        return number * sum;
    }

    private static bool CheckBingos(bool[,] mark)
    {
        var bingo = false;

        for (var y = 0; y < sizeY; ++y)
        {
            var lineBingo = true;

            for (var x = 0; x < sizeX; ++x)
            {
                if (!mark[x, y])
                {
                    lineBingo = false;
                    break;
                }
            }

            if (lineBingo)
            {
                bingo = true;
                break;
            }
        }

        if (!bingo)
        {
            for (var x = 0; x < sizeX; ++x)
            {
                var lineBingo = true;

                for (var y = 0; y < sizeY; ++y)
                {
                    if (!mark[x, y])
                    {
                        lineBingo = false;
                        break;
                    }
                }

                if (lineBingo)
                {
                    bingo = true;
                    break;
                }
            }
        }

        return bingo;
    }
}
