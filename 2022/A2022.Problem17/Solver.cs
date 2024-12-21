using Advent.Common;

namespace A2022.Problem17;

public class Solver : IProblemSolver<long>
{
    public long RunA(string filename)
        => Run(filename, 2022);

    public long RunB(string filename)
        => Run(filename, 1_000_000_000_000);

    static long Run(string filename, long totalFigures)
    {
        var movements = Load(filename);
        var tetris = new Tetris(width: 7, left: 2, topOffset: 3);
        return tetris.Run(movements, totalFigures);
    }

    static Movement[] Load(string filename)
        => File.ReadAllLines(filename)
               .First()
               .ToArray(c => c switch { '>' => Movement.Right, '<' => Movement.Left });
}
