namespace A2022.Problem17;

public static class Solver
{
    [GeneratedTest<long>(3068, 3055)]
    public static long RunA(string[] lines)
        => Run(lines, 2022);

    [GeneratedTest<long>(1514285714288, 1507692307690)]
    public static long RunB(string[] lines)
        => Run(lines, 1_000_000_000_000);

    static long Run(string[] lines, long totalFigures)
    {
        var movements = LoadData(lines);
        var tetris = new Tetris(width: 7, left: 2, topOffset: 3);
        return tetris.Run(movements, totalFigures);
    }

    static Movement[] LoadData(string[] lines)
        => lines[0]
            .ToArray(c => c switch { '>' => Movement.Right, '<' => Movement.Left });
}
