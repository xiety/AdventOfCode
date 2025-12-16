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
            foreach (var i in boards.Length)
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
            foreach (var i in boards.Length)
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

        foreach (var i in marks.Length)
            marks[i] = new bool[SizeX, SizeY];

        return marks;
    }

    static void MarkBoardCell(int[,] boards, bool[,] marks, int number)
    {
        foreach (var y in SizeY)
            foreach (var x in SizeX)
                if (boards[x, y] == number)
                    marks[x, y] = true;
    }

    static int[][,] ParseBoards(string[] items)
    {
        var list = new List<int[,]>();

        foreach (var part in items.SplitBy(String.Empty).Select(a => a.ToArray()))
        {
            var array = new int[SizeX, SizeY];

            foreach (var y in SizeY)
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

        foreach (var y in SizeY)
            foreach (var x in SizeX)
                if (!mark[x, y])
                    sum += board[x, y];

        return number * sum;
    }

    static bool CheckBingos(bool[,] mark)
    {
        var bingo = false;

        foreach (var y in SizeY)
        {
            var lineBingo = true;

            foreach (var x in SizeX)
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
            foreach (var x in SizeX)
            {
                var lineBingo = true;

                foreach (var y in SizeY)
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
