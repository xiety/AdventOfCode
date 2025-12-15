using Advent.Common;

namespace A2021.Problem04;

public static class Solver
{
    const int SizeX = 5;
    const int SizeY = 5;

    [GeneratedTest<int>(4512, 49860)]
    public static int RunA(string[] lines)
    {
        var (numbers, boards) = LoadData(lines);

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

    [GeneratedTest<int>(1924, 24628)]
    public static int RunB(string[] lines)
    {
        var (numbers, boards) = LoadData(lines);

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

                    if (wins.All(a => a))
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

    static bool[][,] CreateEmptyMarks(int[][,] boards)
    {
        var marks = new bool[boards.Length][,];

        for (var i = 0; i < marks.Length; ++i)
            marks[i] = new bool[SizeX, SizeY];

        return marks;
    }

    static void MarkBoardCell(int[,] boards, bool[,] marks, int number)
    {
        for (var y = 0; y < SizeY; ++y)
            for (var x = 0; x < SizeX; ++x)
                if (boards[x, y] == number)
                    marks[x, y] = true;
    }

    static int[][,] ParseBoards(string[] items)
    {
        var list = new List<int[,]>();

        foreach (var part in items.SplitBy(String.Empty).Select(a => a.ToArray()))
        {
            var array = new int[SizeX, SizeY];

            for (var y = 0; y < SizeY; ++y)
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

    static int CalculateResult(int[,] board, bool[,] mark, int number)
    {
        var sum = 0;

        for (var y = 0; y < SizeY; ++y)
            for (var x = 0; x < SizeX; ++x)
                if (!mark[x, y])
                    sum += board[x, y];

        return number * sum;
    }

    static bool CheckBingos(bool[,] mark)
    {
        var bingo = false;

        for (var y = 0; y < SizeY; ++y)
        {
            var lineBingo = true;

            for (var x = 0; x < SizeX; ++x)
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
            for (var x = 0; x < SizeX; ++x)
            {
                var lineBingo = true;

                for (var y = 0; y < SizeY; ++y)
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

    static (int[] numbers, int[][,]) LoadData(string[] lines)
    {
        var numbers = lines[0].Split(',').ToArray(int.Parse);
        var boards = ParseBoards(lines[2..]);

        return (numbers, boards);
    }
}
