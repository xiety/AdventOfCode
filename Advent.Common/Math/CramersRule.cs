namespace Advent.Common;

public static class CramersRule
{
    public static int[] SolveCramer(int[,] matrix, int[] column)
        => Solve(new SubMatrix(matrix, column));

    static int[] Solve(SubMatrix matrix)
    {
        var det = matrix.Det();
        if (det == 0)
            throw new ArgumentException("The determinant is zero.");

        var answer = new int[matrix.Size];

        for (var i = 0; i < matrix.Size; ++i)
        {
            matrix.ColumnIndex = i;
            answer[i] = matrix.Det() / det;
        }

        return answer;
    }

    class SubMatrix
    {
        readonly int[,]? source;
        readonly SubMatrix? prev;
        readonly int[]? replaceColumn;

        public SubMatrix(int[,] source, int[] replaceColumn)
        {
            this.source = source;
            this.replaceColumn = replaceColumn;
            this.prev = null;
            this.ColumnIndex = -1;
            Size = replaceColumn.Length;
        }

        SubMatrix(SubMatrix prev, int deletedColumnIndex = -1)
        {
            this.source = null;
            this.prev = prev;
            this.ColumnIndex = deletedColumnIndex;
            Size = prev.Size - 1;
        }

        public int ColumnIndex { get; set; }
        public int Size { get; }

        public int this[int row, int column]
        {
            get
            {
                if (source is not null)
                    return column == ColumnIndex ? replaceColumn[row] : source[row, column];
                return prev[row + 1, column < ColumnIndex ? column : column + 1];
            }
        }

        public int Det()
        {
            if (Size == 1)
                return this[0, 0];
            if (Size == 2)
                return this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];
            var m = new SubMatrix(this);
            var det = 0;
            var sign = 1;
            for (var c = 0; c < Size; ++c)
            {
                m.ColumnIndex = c;
                var d = m.Det();
                det += this[0, c] * d * sign;
                sign = -sign;
            }
            return det;
        }
    }
}
