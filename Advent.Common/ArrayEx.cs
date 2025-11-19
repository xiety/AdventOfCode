using System.Numerics;
using System.Text;

namespace System;

public static class ArrayEx
{
    public static readonly Pos[] Offsets = [new(-1, 0), new(0, -1), new(1, 0), new(0, 1)];
    public static readonly Pos[] DiagOffsets = [new(-1, -1), new(1, -1), new(1, 1), new(-1, 1)];

    //TODO: to extension
    //warning CS8620: Argument of type 'Solver.NodeType' cannot be used for parameter 'values' of type 'Solver.NodeType[]' in 'IEnumerable<Pos> extension<NodeType>(NodeType[,]).EnumeratePositionsOf(params NodeType[] values)' due to differences in the nullability of reference types.
    public static IEnumerable<Pos> EnumeratePositionsOf<T>(this T[,] array, params T[] values)
    {
        for (var y = 0; y < array.GetLength(1); ++y)
            for (var x = 0; x < array.GetLength(0); ++x)
                if (values.Contains(array[x, y]))
                    yield return new(x, y);
    }

    extension<T>(T obj)
    {
        public bool Inside(params IEnumerable<T> items)
            => items.Contains(obj);
    }

    extension<T>(T[,] array)
    {
        public T[,] Transposed()
        {
            var rows = array.GetLength(0);
            var cols = array.GetLength(1);
            var result = new T[cols, rows];

            for (var row = 0; row < rows; row++)
                for (var col = 0; col < cols; col++)
                    result[col, row] = array[row, col];

            return result;
        }

        public TR[,] ToArray<TR>(Func<T, TR> map)
        {
            var result = new TR[array.GetLength(0), array.GetLength(1)];

            for (var x = 0; x < array.GetLength(0); ++x)
                for (var y = 0; y < array.GetLength(1); ++y)
                    result[x, y] = map(array[x, y]);

            return result;
        }

        public void Fill(T value)
        {
            for (var x = 0; x < array.GetLength(0); ++x)
                for (var y = 0; y < array.GetLength(1); ++y)
                    array[x, y] = value;
        }

        public string ToString(Func<T, string> format)
            => array.ToString(Environment.NewLine, "", format);

        public string ToString(string lineSeparator, string itemSeparator, Func<T, string> format)
        {
            var sb = new StringBuilder();

            for (var y = 0; y < array.GetLength(1); ++y)
            {
                sb.AppendJoin(itemSeparator, array.GetRow(y).Select(format));
                sb.Append(lineSeparator);
            }

            return sb.ToString();
        }

        public void Dump(Func<T, string> format)
            => array.Dump(Environment.NewLine, "", format);

        public void Dump(string lineSeparator, string itemSeparator, Func<T, string> format)
        {
            Console.WriteLine(ToString(array, lineSeparator, itemSeparator, format));
        }

        public IEnumerable<T> GetRow(int row)
        {
            for (var i = 0; i < array.GetLength(0); ++i)
                yield return array[i, row];
        }

        public void SetRow(int row, IEnumerable<T> enumerable)
        {
            var x = 0;

            foreach (var item in enumerable)
            {
                array[x, row] = item;
                x++;
            }
        }

        public IEnumerable<T> GetColumn(int column)
        {
            for (var i = 0; i < array.GetLength(1); ++i)
                yield return array[column, i];
        }

        public IEnumerable<T[]> GetColumns()
        {
            for (var x = 0; x < array.Width; ++x)
                yield return GetColumn(array, x).ToArray();
        }

        public T Get(Pos p)
            => array[p.X, p.Y];

        public T GetOrDefault(Pos p, T defaultValue)
            => array.IsInBounds(p) ? array[p.X, p.Y] : defaultValue;

        public ref T GetRef(Pos p)
            => ref array[p.X, p.Y];

        public T Set(Pos p, T value)
            => array[p.X, p.Y] = value;

        public bool IsInBounds(Pos p)
            => p.X >= 0 && p.X < array.GetLength(0) && p.Y >= 0 && p.Y < array.GetLength(1);

        public IEnumerable<Pos> EnumeratePositions()
        {
            for (var y = 0; y < array.GetLength(1); ++y)
                for (var x = 0; x < array.GetLength(0); ++x)
                    yield return new(x, y);
        }

        public IEnumerable<(Pos Pos, T Item)> Enumerate()
        {
            for (var y = 0; y < array.GetLength(1); ++y)
                for (var x = 0; x < array.GetLength(0); ++x)
                    yield return (new(x, y), array[x, y]);
        }

        public IEnumerable<Pos> EnumeratePositionsOf(T value)
        {
            for (var y = 0; y < array.GetLength(1); ++y)
                for (var x = 0; x < array.GetLength(0); ++x)
                    if (array[x, y]?.Equals(value) ?? false)
                        yield return new(x, y);
        }

        //public IEnumerable<Pos> EnumeratePositionsOf(params T[] values)
        //{
        //    for (var y = 0; y < array.GetLength(1); ++y)
        //        for (var x = 0; x < array.GetLength(0); ++x)
        //            if (values.Contains(array[x, y]))
        //                yield return new(x, y);
        //}

        public Pos FindValue(T value)
        {
            for (var y = 0; y < array.GetLength(1); ++y)
                for (var x = 0; x < array.GetLength(0); ++x)
                    if (array[x, y]?.Equals(value) ?? false)
                        return new(x, y);

            throw new ArgumentOutOfRangeException(paramName: nameof(value));
        }

        public int Width
            => array.GetLength(0);

        public int Height
            => array.GetLength(1);

        public IEnumerable<Pos> Offsetted(Pos center)
            => array.Offsetted(center, Offsets);

        public IEnumerable<Pos> Offsetted(Pos center, IEnumerable<Pos> offsets)
            => offsets
                .Select(a => center + a)
                .Where(array.IsInBounds);

        public void ForEach(Action<Pos> action)
        {
            for (var y = 0; y < array.GetLength(1); ++y)
                for (var x = 0; x < array.GetLength(0); ++x)
                    action(new(x, y));
        }

        public Pos Size()
            => new(array.Width, array.Height);

        public IEnumerable<Pos> Delted(Pos c)
        {
            for (var dx = -1; dx <= 1; ++dx)
            {
                for (var dy = -1; dy <= 1; ++dy)
                {
                    if (dx == 0 && dy == 0)
                        continue;

                    var k = new Pos(dx, dy) + c;

                    if (array.IsInBounds(k))
                        yield return k;
                }
            }
        }
    }

    extension<T>(T[] array)
    {
        public void ForEach(Action<T> action)
        {
            for (var i = 0; i < array.Length; ++i)
                action(array[i]);
        }

        public T[] With(int index, T newValue)
        {
            var newArray = array.ToArray();
            newArray[index] = newValue;
            return newArray;
        }

        public IEnumerable<int> FindAllIndexes(T search)
        {
            for (var i = 0; i < array.Length; ++i)
            {
                if ((array[i] == null && search == null) || (array[i] != null && array[i]!.Equals(search)))
                    yield return i;
            }
        }

        public Dictionary<TKey, T> ToDictionary<TKey>()
            where TKey : INumber<TKey>
           => array.Select((v, i) => KeyValuePair.Create(TKey.CreateChecked(i), v))
            .ToDictionary(a => a.Key, a => a.Value);
    }

    extension<TKey, TValue>(TValue[] array)
        where TKey : INumber<TKey>
    {
        public Dictionary<TKey, TValue> ToDictionary2()
           => array.Select((v, i) => KeyValuePair.Create(TKey.CreateChecked(i), v))
            .ToDictionary(a => a.Key, a => a.Value);
    }

    extension<T>(T[,] array)
        where T : IEquatable<T>
    {
        public bool TryFindSubarray(T[,] small, out Pos? pos)
        {
            var bw = array.Width;
            var bh = array.Height;
            var sw = small.Width;
            var sh = small.Height;
            pos = null;

            for (var i = 0; i <= bw - sw; ++i)
                for (var j = 0; j <= bh - sh; ++j)
                {
                    var match = true;

                    for (var x = 0; x < sw && match; ++x)
                        for (var y = 0; y < sh && match; ++y)
                            if (!array[i + x, j + y].Equals(small[x, y]))
                                match = false;

                    if (match)
                    {
                        pos = new(i, j);
                        return true;
                    }
                }

            return false;
        }
    }

    extension<T>(T[,] array)
    {
        public bool SequenceEquals(T[,] b)
            => array.Rank == b.Rank
                && Enumerable.Range(0, array.Rank).All(d => array.GetLength(d) == b.GetLength(d))
                && array.Cast<T>().SequenceEqual(b.Cast<T>());
    }

    extension(Array)
    {
        public static T[] CreateAndInitialize<T>(int number, Func<int, T> creator)
                => Enumerable.Range(0, number).ToArray(creator);

        public static T[] CreateAndInitialize1D<T>(int number, T initValue)
            => Enumerable.Range(0, number).ToArray(_ => initValue);

        public static T[,] CreateAndInitialize<T>(int width, int height, T value)
        {
            var array = new T[width, height];
            array.Fill(value);
            return array;
        }

        public static T[,] CreateAndInitialize<T>(int width, int height, Func<int, int, T> func)
        {
            var array = new T[width, height];

            for (var y = 0; y < height; ++y)
                for (var x = 0; x < width; ++x)
                    array[x, y] = func(x, y);

            return array;
        }

        public static T[,,] CreateAndInitialize<T>(int d1, int d2, int d3, T value)
        {
            var array = new T[d1, d2, d3];
            array.Fill(value);
            return array;
        }

        public static IEnumerable<Pos> EnumerateDeltas()
        {
            for (var dx = -1; dx <= 1; ++dx)
            {
                for (var dy = -1; dy <= 1; ++dy)
                {
                    if (dx == 0 && dy == 0)
                        continue;

                    yield return new Pos(dx, dy);
                }
            }
        }
    }

    extension<T>(T[,,] array)
    {
        public void Fill(T value)
        {
            for (var x = 0; x < array.GetLength(0); ++x)
                for (var y = 0; y < array.GetLength(1); ++y)
                    for (var z = 0; z < array.GetLength(2); ++z)
                        array[x, y, z] = value;
        }

        public T Get(Pos3 p)
            => array[p.X, p.Y, p.Z];

        public T Set(Pos3 p, T value)
            => array[p.X, p.Y, p.Z] = value;

        public bool IsInBounds(Pos3 p)
            => p.X >= 0 && p.X < array.GetLength(0)
            && p.Y >= 0 && p.Y < array.GetLength(1)
            && p.Z >= 0 && p.Z < array.GetLength(2);
    }

    extension(bool[,] array)
    {
        public void Flood(Pos pos)
        {
            var floodPoints = new List<Pos>() { pos };
            var nextPoints = new List<Pos>();

            do
            {
                var items =
                    from p in floodPoints
                    from p2 in array.Delted(p)
                    where !array.Get(p2)
                    select p2;

                foreach (var p2 in items)
                {
                    array.Set(p2, true);
                    nextPoints.Add(p2);
                }

                (nextPoints, floodPoints) = (floodPoints, nextPoints);
                nextPoints.Clear();
            }
            while (floodPoints.Count > 0);
        }
    }
}
