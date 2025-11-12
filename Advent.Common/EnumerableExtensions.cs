using System.Numerics;
using System.Reflection;

namespace System.Linq;

public static class EnumerableExtensions
{
    extension<T>(IEnumerable<T?> source)
        where T : class
    {
        public IEnumerable<T> WhereNotNull()
            => source.Where(a => a != null)!;
    }

    extension<T>(IEnumerable<T> source)
    {
        public TR[] ToArray<TR>(Func<T, TR> selector)
            => source.Select(selector).ToArray();

        public TR[] ToArray<TR>(Func<T, int, TR> selector)
            => source.Select(selector).ToArray();

        public ValueTuple<T, T> ToTuple2()
        {
            var array = source.Take(2).ToArray();
            return (array[0], array[1]);
        }

        public ValueTuple<T, T, T> ToTuple3()
        {
            var array = source.Take(3).ToArray();
            return (array[0], array[1], array[2]);
        }

        public IEnumerable<T> MinAllBy<TKey>(Func<T, TKey> selector)
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

        public IEnumerable<(T, T)> EnumeratePairs()
            => source.ToArray().EnumeratePairs();

        public int FindLoopIndex()
        {
            var hashSet = new HashSet<T>();
            var index = 0;

            foreach (var item in source)
            {
                if (hashSet.Contains(item))
                    return index;

                hashSet.Add(item);
                index++;
            }

            return -1;
        }

        public IEnumerable<T> Debug(Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
                yield return item;
            }
        }

        public IEnumerable<(T First, T Second)> Chain()
        {
            using var enumerator = source.GetEnumerator();

            if (!enumerator.MoveNext())
                yield break;

            var previous = enumerator.Current;

            while (enumerator.MoveNext())
            {
                yield return (previous, enumerator.Current);
                previous = enumerator.Current;
            }
        }

        public string StringJoin(string separator = "")
            => String.Join(separator, source);

        public string StringJoin(string separator, Func<T, string> selection)
            => String.Join(separator, source.Select(selection));

        public IEnumerable<T> AppendRange(params T[] items)
            => Enumerable.Concat(source, items);

        public IEnumerable<IEnumerable<T>> SplitBy(Func<T, bool> isHeader)
        {
            List<T>? list = null;

            foreach (var line in source)
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

        public IEnumerable<T> Dump()
        {
            Console.WriteLine("Begin");

            foreach (var (index, item) in source.Index())
            {
                Console.WriteLine($"{index}: {item}");
                yield return item;
            }

            Console.WriteLine("End");
        }

        //public static void Deconstruct<TV>(source IEnumerable<TV> source, out TV a0, out IEnumerable<TV> a1)
        //{
        //    a0 = source.First();
        //    a1 = source.Skip(1);
        //}

        public void Deconstruct(out T a0, out T a1)
        {
            var enumerator = source.GetEnumerator();

            Assign(enumerator, out a0);
            Assign(enumerator, out a1);
        }

        public void Deconstruct(out T a0, out T a1, out T a2)
        {
            var enumerator = source.GetEnumerator();

            Assign(enumerator, out a0);
            Assign(enumerator, out a1);
            Assign(enumerator, out a2);
        }

        public void Deconstruct(out T a0, out T a1, out T a2, out T a3)
        {
            var enumerator = source.GetEnumerator();

            Assign(enumerator, out a0);
            Assign(enumerator, out a1);
            Assign(enumerator, out a2);
            Assign(enumerator, out a3);
        }

        public TR Sum<TR>(Func<T, TR> func)
            where TR : INumber<TR>
            => source.Aggregate(TR.Zero, (agg, t) => agg + func(t));

        public TR Mul<TR>(Func<T, TR> func)
            where TR : INumber<TR>
            => source.Aggregate(TR.One, (agg, t) => agg * func(t));
    }

    public static IEnumerable<T> Concat<T>(params IEnumerable<T>[] enumerables)
    {
        foreach (var enumerable in enumerables)
            foreach (var item in enumerable)
                yield return item;
    }

    extension<T>(IReadOnlyList<T> collection)
    {
        public IEnumerable<(T, T)> EnumeratePairs()
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
    }

    extension<T>(IEnumerable<T> source)
        where T : IEqualityOperators<T, T, bool>
    {
        public int FindRepeat()
        {
            List<T> list = [];

            var repeatingCount = 0;
            var repeatingFrom = 0;

            foreach (var number in source)
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
    }

     public static IEnumerable<long> LongRange(long start, long length)
    {
        for (var i = start; i < start + length; ++i)
            yield return i;
    }

    extension<T>(IEnumerable<T> source)
        where T : notnull
    {
        public IEnumerable<T[]> SplitBy(T separator)
            //where TV : IEqualityOperators<TV, TV, bool>
        {
            var list = new List<T>();

            foreach (var item in source)
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
    }

    extension(IEnumerable<object> source)
    {
        public object ToArray(Type elementType)
        {
            var typeConvertedEnumerable = source.Cast(elementType);

            var enumerableType = typeof(Enumerable);
            var flags = BindingFlags.Static | BindingFlags.Public;
            var parameters = new[] { elementType };

            var typeConvertedArray = enumerableType
                .GetMethod(nameof(Enumerable.ToArray), flags)!
                .MakeGenericMethod(parameters)
                .Invoke(null, [typeConvertedEnumerable])!;

            return typeConvertedArray;
        }

        object Cast(Type elementType)
        {
            var enumerableType = typeof(Enumerable);
            var flags = BindingFlags.Static | BindingFlags.Public;
            var parameters = new[] { elementType };

            return enumerableType
                .GetMethod(nameof(Enumerable.Cast), flags)!
                .MakeGenericMethod(parameters)
                .Invoke(null, [source])!;
        }

        public object ToList(Type elementType)
        {
            var typeConvertedEnumerable = source.Cast(elementType);

            var enumerableType = typeof(Enumerable);
            var flags = BindingFlags.Static | BindingFlags.Public;
            var parameters = new[] { elementType };

            var typeConvertedList = enumerableType
                .GetMethod(nameof(Enumerable.ToList), flags)!
                .MakeGenericMethod(parameters)
                .Invoke(null, [typeConvertedEnumerable])!;

            return typeConvertedList;
        }
    }

    extension<T>(IEnumerable<T> source)
        where T : struct
    {
        public T? FirstOrNull()
            => source.Cast<T?>().FirstOrDefault();
    }

    public static IEnumerable<Pos> Range2d(int n1, int n2)
    {
        for (var i1 = 0; i1 < n1; ++i1)
            for (var i2 = 0; i2 < n2; ++i2)
                yield return new(i1, i2);
    }

    //public static void Deconstruct(source IEnumerable<int> @source, out Pos pos)
    //{
    //    var enumerator = @source.GetEnumerator();

    //    Assign(enumerator, out var a);
    //    Assign(enumerator, out var y);

    //    pos = new(a, y);
    //}

    static void Assign<T>(IEnumerator<T> enumerator, out T value)
    {
        var next = enumerator.MoveNext();

        if (!next)
            throw new Exception("No data");

        value = enumerator.Current;
    }

    extension<T>(IEnumerable<T> source)
        where T : INumber<T>
    {
        public T Sum()
            => source.Aggregate(T.Zero, (agg, t) => agg + t);

        public T Mul()
            => source.Aggregate(T.One, (agg, t) => agg * t);

        public long MulLong()
            => source.Aggregate(long.CreateChecked(T.One), (agg, t) => agg * long.CreateChecked(t));
    }
}
