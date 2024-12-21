using System.Numerics;
using System.Reflection;

namespace System.Linq;

public static class EnumerableExtensions
{
    public static TR[] ToArray<T, TR>(this IEnumerable<T> source, Func<T, TR> selector)
        => source.Select(selector).ToArray();

    public static TR[] ToArray<T, TR>(this IEnumerable<T> source, Func<T, int, TR> selector)
            => source.Select(selector).ToArray();

    public static IEnumerable<T> MinAllBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector)
    {
        var comparer = Comparer<TKey>.Default;

        var first = true;
        var minValue = default(TKey);
        var result = new List<T>();

        foreach (var item in source)
        {
            var value = selector(item);

            var c = first ? -1 : comparer.Compare(value, minValue);

            if (c < 0)
            {
                minValue = value;
                result.Clear();
                result.Add(item);
                first = false;
            }
            else if (c == 0)
            {
                result.Add(item);
            }
        }

        return result;
    }

    public static IEnumerable<T> Concat<T>(params IEnumerable<T>[] enumerables)
    {
        foreach (var enumerable in enumerables)
            foreach (var item in enumerable)
                yield return item;
    }

    public static IEnumerable<(T, T)> EnumeratePairs<T>(this IReadOnlyList<T> collection)
    {
        for (var i = 0; i < collection.Count - 1; ++i)
        {
            for (var j = i + 1; j < collection.Count; ++j)
            {
                var a = collection[i];
                var b = collection[j];

                yield return (a, b);
            }
        }
    }

    public static IEnumerable<(T, T)> EnumeratePairs<T>(this IEnumerable<T> enumerable)
        => enumerable.ToArray().EnumeratePairs();

    public static int FindLoopIndex<T>(this IEnumerable<T> enumerable)
    {
        var hashSet = new HashSet<T>();
        var index = 0;

        foreach (var item in enumerable)
        {
            if (hashSet.Contains(item))
                return index;

            hashSet.Add(item);
            index++;
        }

        return -1;
    }

    public static int FindRepeat<T>(this IEnumerable<T> enumerable)
        where T : IEqualityOperators<T, T, bool>
    {
        List<T> list = [];

        var repeatingCount = 0;
        var repeatingFrom = 0;

        foreach (var number in enumerable)
        {
            if (list.Count > 0)
            {
                if (list[repeatingCount] == number)
                {
                    if (repeatingCount == 0)
                        repeatingFrom = list.Count;

                    repeatingCount++;
                }
                else
                {
                    repeatingCount = 0;
                    repeatingFrom = -1;
                }

                if (repeatingCount == repeatingFrom)
                    return repeatingCount;
            }

            list.Add(number);
        }

        return 0;
    }

    public static IEnumerable<IEnumerable<T>> Permutations<T>(this IReadOnlyCollection<T> source, int length)
    {
        if (length == 1)
            return source.Select(t => new[] { t });

        return Permutations(source, length - 1)
            .SelectMany(t => source.Where(e => !t.Contains(e)), (t1, t2) => t1.Append(t2));
    }

    public static IEnumerable<long> LongRange(long start, long length)
    {
        for (var i = start; i < start + length; ++i)
            yield return i;
    }

    public static IEnumerable<T> Debug<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var item in enumerable)
        {
            action(item);
            yield return item;
        }
    }

    public static IEnumerable<T[]> Split<T>(this IEnumerable<T> items, T separator)
        where T : notnull
        //where T : IEqualityOperators<T, T, bool>
    {
        var list = new List<T>();

        foreach (var item in items)
        {
            if (item.Equals(separator))
            {
                yield return list.ToArray();
                list.Clear();
            }
            else
            {
                list.Add(item);
            }
        }

        if (list.Count != 0)
            yield return list.ToArray();
    }

    public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> items, Func<T, bool> isHeader)
    {
        List<T>? list = null;

        foreach (var line in items)
        {
            if (isHeader(line))
            {
                if (list != null && list.Count != 0)
                    yield return list;

                list = [];
            }

            list?.Add(line);
        }

        if (list != null && list.Count != 0)
            yield return list;
    }

    public static object ToArray(this IEnumerable<object> enumerable, Type elementType)
    {
        var typeConvertedEnumerable = enumerable.Cast(elementType);

        var enumerableType = typeof(Enumerable);
        var flags = BindingFlags.Static | BindingFlags.Public;
        var parameters = new[] { elementType };

        var typeConvertedArray = enumerableType
            .GetMethod(nameof(Enumerable.ToArray), flags)!
            .MakeGenericMethod(parameters)
            .Invoke(null, [typeConvertedEnumerable])!;

        return typeConvertedArray;
    }

    static object Cast(this IEnumerable<object> enumerable, Type elementType)
    {
        var enumerableType = typeof(Enumerable);
        var flags = BindingFlags.Static | BindingFlags.Public;
        var parameters = new[] { elementType };

        return enumerableType
            .GetMethod(nameof(Enumerable.Cast), flags)!
            .MakeGenericMethod(parameters)
            .Invoke(null, [enumerable])!;
    }

    public static object ToList(this IEnumerable<object> enumerable, Type elementType)
    {
        var typeConvertedEnumerable = enumerable.Cast(elementType);

        var enumerableType = typeof(Enumerable);
        var flags = BindingFlags.Static | BindingFlags.Public;
        var parameters = new[] { elementType };

        var typeConvertedList = enumerableType
            .GetMethod(nameof(Enumerable.ToList), flags)!
            .MakeGenericMethod(parameters)
            .Invoke(null, [typeConvertedEnumerable])!;

        return typeConvertedList;
    }

    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> enumerable)
        => enumerable.Where(a => a != null)!;

    public static T? FirstOrNull<T>(this IEnumerable<T> enumerable)
        where T : struct
        => enumerable.Cast<T?>().FirstOrDefault();

    public static IEnumerable<(T First, T Second)> Chain<T>(this IEnumerable<T> enumerable)
    {
        using var enumerator = enumerable.GetEnumerator();

        if (!enumerator.MoveNext())
            yield break;

        var previous = enumerator.Current;

        while (enumerator.MoveNext())
        {
            yield return (previous, enumerator.Current);
            previous = enumerator.Current;
        }
    }

    public static string StringJoin<T>(this IEnumerable<T> enumerable, string separator = "")
        => String.Join(separator, enumerable);

    public static string StringJoin<T>(this IEnumerable<T> enumerable, string separator, Func<T, string> selection)
        => String.Join(separator, enumerable.Select(selection));

    public static IEnumerable<T> Append<T>(this IEnumerable<T> enumerable, params T[] items)
        => Enumerable.Concat(enumerable, items);

    public static IEnumerable<Pos> Range2d(int n1, int n2)
    {
        for (var i1 = 0; i1 < n1; ++i1)
            for (var i2 = 0; i2 < n2; ++i2)
                yield return new(i1, i2);
    }

    //public static void Deconstruct<T>(this IEnumerable<T> @this, out T a0, out IEnumerable<T> a1)
    //{
    //    a0 = @this.First();
    //    a1 = @this.Skip(1);
    //}

    public static void Deconstruct<T>(this IEnumerable<T> @this, out T a0, out T a1)
    {
        var enumerator = @this.GetEnumerator();

        Assign(enumerator, out a0);
        Assign(enumerator, out a1);
    }

    public static void Deconstruct<T>(this IEnumerable<T> @this, out T a0, out T a1, out T a2)
    {
        var enumerator = @this.GetEnumerator();

        Assign(enumerator, out a0);
        Assign(enumerator, out a1);
        Assign(enumerator, out a2);
    }

    public static void Deconstruct<T>(this IEnumerable<T> @this, out T a0, out T a1, out T a2, out T a3)
    {
        var enumerator = @this.GetEnumerator();

        Assign(enumerator, out a0);
        Assign(enumerator, out a1);
        Assign(enumerator, out a2);
        Assign(enumerator, out a3);
    }

    //public static void Deconstruct(this IEnumerable<int> @this, out Pos pos)
    //{
    //    var enumerator = @this.GetEnumerator();

    //    Assign(enumerator, out var x);
    //    Assign(enumerator, out var y);

    //    pos = new(x, y);
    //}

    static void Assign<T>(IEnumerator<T> enumerator, out T value)
    {
        var next = enumerator.MoveNext();

        if (!next)
            throw new Exception("No data");

        value = enumerator.Current;
    }

    public static TR Sum<T, TR>(this IEnumerable<T> @this, Func<T, TR> func)
        where TR : INumber<TR>
        => @this.Aggregate(TR.Zero, (agg, t) => agg + func(t));

    public static T Sum<T>(this IEnumerable<T> @this)
        where T : INumber<T>
        => @this.Aggregate(T.Zero, (agg, t) => agg + t);

    public static TR Mul<T, TR>(this IEnumerable<T> @this, Func<T, TR> func)
        where TR : INumber<TR>
        => @this.Aggregate(TR.One, (agg, t) => agg * func(t));

    public static T Mul<T>(this IEnumerable<T> @this)
        where T : INumber<T>
        => @this.Aggregate(T.One, (agg, t) => agg * t);

    public static long MulLong<T>(this IEnumerable<T> @this)
        where T : INumber<T>
        => @this.Aggregate(long.CreateChecked(T.One), (agg, t) => agg * long.CreateChecked(t));

    public static IEnumerable<T> Dump<T>(this IEnumerable<T> items)
    {
        Console.WriteLine("Begin");

        foreach (var (index, item) in items.Index())
        {
            Console.WriteLine($"{index}: {item}");
            yield return item;
        }

        Console.WriteLine("End");
    }
}
