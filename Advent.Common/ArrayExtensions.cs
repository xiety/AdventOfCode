using System.Text;

namespace System;

public static class ArrayEx
{
    public static T[] CreateAndInitialize<T>(int number, Func<int, T> creator)
        => Enumerable.Range(0, number).Select(creator).ToArray();

    public static T[] CreateAndInitialize<T>(int number, T value)
        => Enumerable.Range(0, number).Select(_ => value).ToArray();

    public static T[,] CreateAndInitialize<T>(int width, int height, T value)
    {
        var array = new T[width, height];
        array.Fill(value);
        return array;
    }

    public static T[,,] CreateAndInitialize<T>(int d1, int d2, int d3, T value)
    {
        var array = new T[d1, d2, d3];
        array.Fill(value);
        return array;
    }
}

public static class ArrayExtensions
{
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

    public static void Dump<T>(this T[,] array, string format)
        where T : IFormattable
    {
        for (var y = 0; y < array.GetLength(1); ++y)
        {
            Console.WriteLine(String.Join(", ", array.GetRow(y).Select(a => a.ToString(format, null))));
            Console.WriteLine();
        }

        Console.WriteLine();
    }

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

    public static void Dump<T>(this T[,] array, string lineSeparator, string itemSeparator, Func<T, string> format)
    {
        Console.WriteLine(ToString(array, lineSeparator, itemSeparator, format));
    }

    public static void Dump<T>(this T[,] array, string format, string separator, string lineSeparator)
        where T : IFormattable
    {
        for (var y = 0; y < array.GetLength(1); ++y)
        {
            Console.Write(String.Join(separator, array.GetRow(y).Select(a => a.ToString(format, null))));
            Console.Write(lineSeparator);
        }

        Console.WriteLine();
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

    public static IEnumerable<(Pos pos, T item)> Enumerate<T>(this T[,] array)
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

    public static IEnumerable<Pos> EnumerateNearest<T>(this T[,] array, Pos center)
        => EnumerableExtensions
              .Range2d(3, 3)
              .Select(a => center + a + new Pos(-1, -1))
              .Where(a => array.IsInBounds(a));

    private static readonly Pos[] offsets = [new(-1, 0), new(0, -1), new(1, 0), new(0, 1)];

    public static IEnumerable<Pos> Offsets<T>(this T[,] array, Pos center)
        => offsets
              .Select(a => center + a)
              .Where(a => array.IsInBounds(a));

    public static void ForEach<T>(this T[,] array, Action<Pos> action)
    {
        for (var y = 0; y < array.GetLength(1); ++y)
            for (var x = 0; x < array.GetLength(0); ++x)
                action(new(x, y));
    }
}
