using System.Numerics;
using System.Reflection;

namespace System.Linq;

public static class EnumerableExtensions
{
    public static IEnumerable<(TItem item, int index)> Indexed<TItem>(this IEnumerable<TItem> items)
        => items.Select((item, index) => (item, index));

    public static IEnumerable<T> Debug<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var item in enumerable)
        {
            action(item);
            yield return item;
        }
    }

    public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> items, T separator)
        where T : notnull
        //where T : IEqualityOperators<T, T, bool>
    {
        var list = new List<T>();

        foreach (var item in items)
        {
            if (item.Equals(separator))
            {
                yield return list;
                list = [];
            }
            else
            {
                list.Add(item);
            }
        }

        if (list.Any())
            yield return list;
    }

    public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> items, Func<T, bool> isHeader)
    {
        List<T>? list = null;

        foreach (var line in items)
        {
            if (isHeader(line))
            {
                if (list != null && list.Any())
                    yield return list;

                list = [];
            }

            list?.Add(line);
        }

        if (list != null && list.Any())
            yield return list;
    }

    public static object ToArray(this IEnumerable<object> enumerable, Type elementType)
    {
        var typeConvertedEnumerable = enumerable.Cast(elementType);

        var enumerableType = typeof(Enumerable);
        var flags = BindingFlags.Static | BindingFlags.Public;
        var parameters = new Type[] { elementType };

        var typeConvertedArray = enumerableType
            .GetMethod(nameof(Enumerable.ToArray), flags)!
            .MakeGenericMethod(parameters)
            .Invoke(null, new object[] { typeConvertedEnumerable })!;

        return typeConvertedArray;
    }

    private static object Cast(this IEnumerable<object> enumerable, Type elementType)
    {
        var enumerableType = typeof(Enumerable);
        var flags = BindingFlags.Static | BindingFlags.Public;
        var parameters = new Type[] { elementType };

        return enumerableType
            .GetMethod(nameof(Enumerable.Cast), flags)!
            .MakeGenericMethod(parameters)
            .Invoke(null, new object[] { enumerable })!;
    }

    public static object ToList(this IEnumerable<object> enumerable, Type elementType)
    {
        var typeConvertedEnumerable = enumerable.Cast(elementType);

        var enumerableType = typeof(Enumerable);
        var flags = BindingFlags.Static | BindingFlags.Public;
        var parameters = new Type[] { elementType };

        var typeConvertedList = enumerableType
            .GetMethod(nameof(Enumerable.ToList), flags)!
            .MakeGenericMethod(parameters)
            .Invoke(null, new object[] { typeConvertedEnumerable })!;

        return typeConvertedList;
    }

    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> enumerable)
        => enumerable.Where(a => a != null)!;

    public static T? FirstOrNull<T>(this IEnumerable<T> enumerable)
        where T : struct
        => enumerable.Cast<T?>().FirstOrDefault();

    public static IEnumerable<(T, T)> Chain<T>(this IEnumerable<T> enumerable)
    {
        var first = enumerable.First();

        foreach (var item in enumerable.Skip(1))
        {
            yield return (first, item);
            first = item;
        }
    }

    public static string StringJoin<T>(this IEnumerable<T> enumerable, string separator = "")
        => String.Join(separator, enumerable);

    public static string StringJoin<T>(this IEnumerable<T> enumerable, string separator, Func<T, string> selection)
        => String.Join(separator, enumerable.Select(selection));

    public static IEnumerable<T> Concat<T>(this IEnumerable<T> enumerable, params T[] items)
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

    private static void Assign<T>(IEnumerator<T> enumerator, out T value)
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
}
