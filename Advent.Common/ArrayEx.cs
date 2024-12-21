using System.Numerics;
using System.Text;

namespace System;

public static class ArrayEx
{
    public static readonly Pos[] Offsets = [new(-1, 0), new(0, -1), new(1, 0), new(0, 1)];
    public static readonly Pos[] DiagOffsets = [new(-1, -1), new(1, -1), new(1, 1), new(-1, 1)];

    public static T[,] Transposed<T>(this T[,] array)
    {
        var rows = array.GetLength(0);
        var cols = array.GetLength(1);
        var result = new T[cols, rows];

        for (int row = 0; row < rows; row++)
            for (int col = 0; col < cols; col++)
                result[col, row] = array[row, col];

        return result;
    }

    public static TR[,] ToArray<T, TR>(this T[,] array, Func<T, TR> map)
    {
        var result = new TR[array.GetLength(0), array.GetLength(1)];

        for (var x = 0; x < array.GetLength(0); ++x)
            for (var y = 0; y < array.GetLength(1); ++y)
                result[x, y] = map(array[x, y]);

        return result;
    }

    public static void ForEach<T>(this T[] array, Action<T> action)
    {
        for (var i = 0; i < array.Length; ++i)
            action(array[i]);
    }

    public static bool TryFindSubarray<T>(this T[,] big, T[,] small, out Pos? pos)
        where T : IEquatable<T>
    {
        var bw = big.GetWidth();
        var bh = big.GetHeight();
        var sw = small.GetWidth();
        var sh = small.GetHeight();
        pos = null;

        for (var i = 0; i <= bw - sw; ++i)
            for (var j = 0; j <= bh - sh; ++j)
            {
                var match = true;

                for (var x = 0; x < sw && match; ++x)
                    for (var y = 0; y < sh && match; ++y)
                        if (!big[i + x, j + y].Equals(small[x, y]))
                            match = false;

                if (match)
                {
                    pos = new(i, j);
                    return true;
                }
            }

        return false;
    }

    public static bool SequenceEquals<T>(this T[,] a, T[,] b)
        => a.Rank == b.Rank
        && Enumerable.Range(0, a.Rank).All(d => a.GetLength(d) == b.GetLength(d))
        && a.Cast<T>().SequenceEqual(b.Cast<T>());

    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this TValue[] array)
        where TKey : INumber<TKey>
        => array.Select((v, i) => KeyValuePair.Create(TKey.CreateChecked(i), v))
                .ToDictionary(a => a.Key, a => a.Value);

    public static T[] CreateAndInitialize<T>(int number, Func<int, T> creator)
            => Enumerable.Range(0, number).ToArray(creator);

    public static T[] CreateAndInitialize<T>(int number, T value)
        => Enumerable.Range(0, number).ToArray(_ => value);

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

    public static T[] With<T>(this T[] array, int index, T newValue)
    {
        var newArray = array.ToArray();
        newArray[index] = newValue;
        return newArray;
    }

    public static void Fill<T>(this T[,] array, T value)
    {
        for (var x = 0; x < array.GetLength(0); ++x)
            for (var y = 0; y < array.GetLength(1); ++y)
                array[x, y] = value;
    }

    public static void Fill<T>(this T[,,] array, T value)
    {
        for (var x = 0; x < array.GetLength(0); ++x)
            for (var y = 0; y < array.GetLength(1); ++y)
                for (var z = 0; z < array.GetLength(2); ++z)
                    array[x, y, z] = value;
    }

    public static string ToString<T>(this T[,] array, Func<T, string> format)
        => array.ToString(Environment.NewLine, "", format);

    public static string ToString<T>(this T[,] array, string lineSeparator, string itemSeparator, Func<T, string> format)
    {
        var sb = new StringBuilder();

        for (var y = 0; y < array.GetLength(1); ++y)
        {
            sb.Append(String.Join(itemSeparator, array.GetRow(y).Select(format)));
            sb.Append(lineSeparator);
        }

        return sb.ToString();
    }

    public static void Dump<T>(this T[,] array, Func<T, string> format)
        => array.Dump(Environment.NewLine, "", format);

    public static void Dump<T>(this T[,] array, string lineSeparator, string itemSeparator, Func<T, string> format)
    {
        Console.WriteLine(ToString(array, lineSeparator, itemSeparator, format));
    }

    public static IEnumerable<T> GetRow<T>(this T[,] array, int row)
    {
        for (var i = 0; i < array.GetLength(0); ++i)
            yield return array[i, row];
    }

    public static void SetRow<T>(this T[,] array, int row, IEnumerable<T> enumerable)
    {
        var x = 0;

        foreach (var item in enumerable)
        {
            array[x, row] = item;
            x++;
        }
    }

    public static IEnumerable<T> GetColumn<T>(this T[,] array, int column)
    {
        for (var i = 0; i < array.GetLength(1); ++i)
            yield return array[column, i];
    }

    public static T Get<T>(this T[,] array, Pos p)
        => array[p.X, p.Y];

    public static T GetOrDefault<T>(this T[,] array, Pos p, T defaultValue)
        => array.IsInBounds(p) ? array[p.X, p.Y] : defaultValue;

    public static ref T GetRef<T>(this T[,] array, Pos p)
        => ref array[p.X, p.Y];

    public static T Set<T>(this T[,] array, Pos p, T value)
        => array[p.X, p.Y] = value;

    public static T Get<T>(this T[,,] array, Pos3 p)
        => array[p.X, p.Y, p.Z];

    public static T Set<T>(this T[,,] array, Pos3 p, T value)
        => array[p.X, p.Y, p.Z] = value;

    public static bool IsInBounds<T>(this T[,] array, Pos p)
        => p.X >= 0 && p.X < array.GetLength(0) && p.Y >= 0 && p.Y < array.GetLength(1);

    public static bool IsInBounds<T>(this T[,,] array, Pos3 p)
        => p.X >= 0 && p.X < array.GetLength(0)
        && p.Y >= 0 && p.Y < array.GetLength(1)
        && p.Z >= 0 && p.Z < array.GetLength(2);

    public static IEnumerable<Pos> EnumeratePositions<T>(this T[,] array)
    {
        for (var y = 0; y < array.GetLength(1); ++y)
            for (var x = 0; x < array.GetLength(0); ++x)
                yield return new(x, y);
    }

    public static IEnumerable<(Pos Pos, T Item)> Enumerate<T>(this T[,] array)
    {
        for (var y = 0; y < array.GetLength(1); ++y)
            for (var x = 0; x < array.GetLength(0); ++x)
                yield return (new(x, y), array[x, y]);
    }

    public static IEnumerable<Pos> EnumeratePositionsOf<T>(this T[,] array, T value)
    {
        for (var y = 0; y < array.GetLength(1); ++y)
            for (var x = 0; x < array.GetLength(0); ++x)
                if (array[x, y]?.Equals(value) ?? false)
                    yield return new(x, y);
    }

    public static IEnumerable<Pos> EnumeratePositionsOf<T>(this T[,] array, params T[] values)
    {
        for (var y = 0; y < array.GetLength(1); ++y)
            for (var x = 0; x < array.GetLength(0); ++x)
                if (values.Contains(array[x, y]))
                    yield return new(x, y);
    }

    public static Pos FindValue<T>(this T[,] array, T value)
    {
        for (var y = 0; y < array.GetLength(1); ++y)
            for (var x = 0; x < array.GetLength(0); ++x)
                if (array[x, y]?.Equals(value) ?? false)
                    return new(x, y);

        throw new ArgumentOutOfRangeException();
    }

    public static IEnumerable<int> FindAllIndexes<T>(this T[] array, T search)
    {
        for (var i = 0; i < array.Length; ++i)
        {
            if ((array[i] == null && search == null) || (array[i] != null && array[i]!.Equals(search)))
                yield return i;
        }
    }

    public static int GetWidth<T>(this T[,] array)
        => array.GetLength(0);

    public static int GetHeight<T>(this T[,] array)
        => array.GetLength(1);

    public static IEnumerable<Pos> Offsetted(Pos center)
        => Offsets
               .Select(a => center + a);

    public static IEnumerable<Pos> Offsetted<T>(this T[,] array, Pos center)
        => Offsetted(center)
               .Where(array.IsInBounds);

    public static IEnumerable<Pos> Offsetted<T>(this T[,] array, Pos center, IEnumerable<Pos> offsets)
        => offsets
               .Select(a => center + a)
               .Where(array.IsInBounds);

    public static void ForEach<T>(this T[,] array, Action<Pos> action)
    {
        for (var y = 0; y < array.GetLength(1); ++y)
            for (var x = 0; x < array.GetLength(0); ++x)
                action(new(x, y));
    }

    public static Pos Size<T>(this T[,] array)
        => new(array.GetWidth(), array.GetHeight());

    public static void Flood(this bool[,] array, Pos pos)
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

    public static IEnumerable<Pos> Delted<T>(this T[,] array, Pos c)
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
