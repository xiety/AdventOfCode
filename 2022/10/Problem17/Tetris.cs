namespace A2022.Problem17;

public class Tetris(int width, int left, int topOffset)
{
    readonly List<char[]> glass = [];
    readonly char[] emptyRow = Array.CreateAndInitialize1D(width, '.');
    readonly Dictionary<string, (long, long)> archive = [];

    readonly string[][] figures =
    [
        [ "####",
        ],
        [ ".#.",
          "###",
          ".#.",
        ],
        [ "..#",
          "..#",
          "###",
        ],
        [ "#",
          "#",
          "#",
          "#",
        ],
        [ "##",
          "##",
        ],
    ];

    public long Run(Movement[] movements, long totalFigures)
    {
        var movementIndex = 0;
        var figureNumber = 0;
        var cutted = 0L;
        var foundSame = false;

        for (var step = 0L; step < totalFigures; ++step)
        {
            var cuttedDelta = 0L;
            (movementIndex, cuttedDelta) = Simulate(figureNumber, movements, movementIndex);

            cutted += cuttedDelta;

            if (!foundSame)
                (step, cutted, foundSame) = FindSame(step, cutted, movementIndex, figureNumber, totalFigures);

            figureNumber++;
            if (figureNumber >= figures.Length)
                figureNumber = 0;
        }

        var highest = FindHighest();

        return highest + cutted + 1;
    }

    (long, long, bool) FindSame(long step, long cutted, int movementIndex, int figureNumber, long totalFigures)
    {
        var text = CreateKey(movementIndex, figureNumber);
        var foundSame = false;

        if (archive.TryGetValue(text, out var original))
        {
            var (originalStep, originalCutted) = original;

            var repetitions = (totalFigures - step) / (step - originalStep);

            step += repetitions * (step - originalStep);
            cutted += repetitions * (cutted - originalCutted);

            foundSame = true;
        }
        else
        {
            archive.Add(text, (step, cutted));
        }

        return (step, cutted, foundSame);
    }

    string CreateKey(int currentMovementIndex, int figureNumber)
    {
        var glassText = glass.Select(line => line.StringJoin()).StringJoin(";");
        return $"{currentMovementIndex};{figureNumber};{glassText}";
    }

    (int, long) Simulate(int figureNumber, Movement[] movements, int movementIndex)
    {
        var figure = figures[figureNumber];

        Resize(figure.Length);

        var position = PutFigure(figure);

        var size = figure.Length;

        do
        {
            var movement = movements[movementIndex];

            movementIndex++;
            if (movementIndex >= movements.Length)
                movementIndex = 0;

            if (CanMove(movement, position, size))
                Move(movement, position, size);

            var canFall = CanFall(position, size);

            if (!canFall)
                break;

            Fall(position, size);

            position--;
        }
        while (true);

        Freeze(position, size);

        var cuttedDelta = Cut(position, size);

        return (movementIndex, cuttedDelta);
    }

    bool CanMove(Movement movement, int from, int size)
    {
        var offset = movement == Movement.Left ? -1 : 1;
        var limit = movement == Movement.Left ? 0 : width - 1;

        for (var i = from; i >= from - size + 1; --i)
        {
            var line = glass[i];

            foreach (var j in line.Length)
                if (line[j] == '@')
                    if (j == limit || line[j + offset] == '#')
                        return false;
        }

        return true;
    }

    void Move(Movement movement, int from, int size)
    {
        for (var i = from; i >= from - size + 1; --i)
        {
            var line = glass[i];

            if (movement == Movement.Left)
            {
                foreach (var index in 1..line.Length)
                {
                    if (line[index] == '@')
                    {
                        line[index - 1] = '@';
                        line[index] = '.';
                    }
                }
            }
            else if (movement == Movement.Right)
            {
                for (var index = line.Length - 2; index >= 0; --index)
                {
                    if (line[index] == '@')
                    {
                        line[index + 1] = '@';
                        line[index] = '.';
                    }
                }
            }
        }
    }

    bool CanFall(int from, int size)
    {
        for (var i = from; i >= from - size + 1; --i)
        {
            var line = glass[i];

            foreach (var j in line.Length)
            {
                if (line[j] == '@')
                {
                    if (i == 0)
                        return false;

                    if (glass[i - 1][j] == '#')
                        return false;
                }
            }
        }

        return true;
    }

    void Fall(int from, int size)
    {
        //normal direction
        foreach (var i in (from - size + 1)..(from + 1))
        {
            var line = glass[i];

            foreach (var j in line.Length)
            {
                if (line[j] == '@')
                {
                    glass[i - 1][j] = '@';
                    glass[i][j] = '.';
                }
            }
        }
    }

    void Freeze(int from, int size)
    {
        for (var i = from; i >= from - size + 1; --i)
        {
            var line = glass[i];

            foreach (var index in line.Length)
                if (line[index] == '@')
                    line[index] = '#';
        }
    }

    int Cut(int from, int size)
    {
        var lowest = FindPathFromLeftToRight(from, size);
        var cut = 0;

        if (lowest < glass.Count)
        {
            cut = lowest;
            glass.RemoveRange(0, lowest);
        }

        return cut;
    }

    void Resize(int figureHeight)
    {
        //enlarge
        var highest = FindHighest();
        var requiredHeight = figureHeight + topOffset;
        var difference = highest + requiredHeight - glass.Count + 1;

        if (difference > 0)
            glass.AddRange(Enumerable.Range(0, difference).Select(_ => emptyRow.ToArray()));
    }

    int PutFigure(string[] figure)
    {
        var highest = FindHighest();
        var figureHeight = figure.Length;
        var requiredHeight = figureHeight + topOffset;

        for (var i = figure.Length - 1; i >= 0; --i)
        {
            foreach (var j in figure[i].Length)
            {
                if (figure[i][j] == '#')
                    glass[highest + requiredHeight - i][j + left] = '@';
            }
        }

        return highest + requiredHeight;
    }

    int FindHighest()
    {
        for (var i = glass.Count - 1; i >= 0; --i)
            if (glass[i].Contains('#'))
                return i;

        return -1;
    }

    int FindPathFromLeftToRight(int from, int size)
    {
        for (var y = Math.Min(glass.Count - 1, from + size + 1); y >= Math.Max(1, from - 1); --y)
        {
            var full = true;

            foreach (var x in glass[y].Length)
            {
                if (glass[y][x] != '#' && glass[y - 1][x] != '#')
                {
                    full = false;
                    break;
                }
            }

            if (full)
                return y - 1;
        }

        return glass.Count;
    }
}

public enum Movement
{
    Left,
    Right,
}
