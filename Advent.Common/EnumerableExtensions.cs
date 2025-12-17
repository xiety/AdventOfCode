using System.Numerics;
using System.Reflection;

namespace System.Linq;

public static class EnumerableExtensions
{
    extension(Enumerable)
    {
        public static IEnumerable<T> RangeTo<T>(T startIncl, T endExcl)
            where T : INumber<T>
        {
            for (var i = startIncl; i < endExcl; ++i)
                yield return i;
        }

        public static IEnumerable<T> RangeTo<T>(T startIncl, T endExcl, T step)
            where T : INumber<T>
        {
            for (var i = startIncl; i < endExcl; i += step)
                yield return i;
        }
    }

    extension<T>(IEnumerable<T?> source)
        where T : class
    {
        public IEnumerable<T> WhereNotNull()
            => source.Where(a => a is not null)!;
    }

    extension<T>(IEnumerable<T> source)
    {
        public IEnumerable<(T First, T Second)> EnumeratePairs()
            => source.SelectMany((a, i) => source.Skip(i + 1).Select(b => (a, b)));

        public void Foreach(Action<T> action)
        {
            foreach (var item in source)
                action(item);
        }

        public IEnumerable<T> Do(Action<T> action)
            => source.Select(a => { action(a); return a; });

        public IEnumerable<T> TakeNLowest<TPriority>(Func<T, TPriority> prioritySelector, int n)
        {
            var comparer = Comparer<TPriority>.Default;
            var maxHeap = new PriorityQueue<T, TPriority>(n, Comparer<TPriority>.Create((p1, p2) => comparer.Compare(p2, p1)));

            foreach (var item in source)
            {
                var priority = prioritySelector(item);

                if (maxHeap.Count < n)
                    maxHeap.Enqueue(item, priority);
                else if (maxHeap.TryPeek(out _, out var highestPriorityInHeap) && comparer.Compare(priority, highestPriorityInHeap) < 0)
                    maxHeap.EnqueueDequeue(item, priority);
            }

            return maxHeap.UnorderedItems.Select(x => x.Element);
        }

        public IEnumerable<TResult> SelectPrevCurrNext<TResult>(
            Func<T?, T, T?, TResult> selector)
        {
            using var e = source.GetEnumerator();
            if (!e.MoveNext())
                yield break;

            T? prev = default;
            var curr = e.Current;

            while (e.MoveNext())
            {
                var next = e.Current;
                yield return selector(prev, curr, next);
                prev = curr;
                curr = next;
            }

            yield return selector(prev, curr, default);
        }

        public IEnumerable<T> Slice(int start, int length)
            => source.Skip(start).Take(length);

        public IEnumerable<T> SliceTo(int fromIncl, int toExcl)
            => source.Skip(fromIncl).Take(toExcl - fromIncl);

        public IEnumerable<T> Slice(Range range)
        {
            var count = int.MaxValue;

            if (range.Start.IsFromEnd || range.End.IsFromEnd)
            {
                if (!source.TryGetNonEnumeratedCount(out count))
                    throw new ArgumentOutOfRangeException(nameof(range));
            }

            var (start, length) = range.GetOffsetAndLength(count);
            return source.Slice(start, length);
        }

        public IEnumerable<TR> Accumulate<TR>(TR seed, Func<TR, T, TR> acc)
        {
            var current = seed;
            yield return current;

            foreach (var item in source)
            {
                current = acc(current, item);
                yield return current;
            }
        }

        public IEnumerable<HeaderGrouping<TKey, TValue>> GroupByHeader<TKey, TValue>(Func<T, TKey?> keySelector, Func<T, TValue?> valueSelector)
            where TKey : class
            where TValue : class
        {
            TKey? currentHeader = null;
            var currentItems = new List<TValue>();

            foreach (var item in source)
            {
                if (keySelector(item) is { } newHeader)
                {
                    if (currentHeader is not null)
                        yield return new(currentHeader, [.. currentItems]);

                    currentHeader = newHeader;
                    currentItems.Clear();
                }
                else
                {
                    if (valueSelector(item) is { } v)
                        currentItems.Add(v);
                }
            }

            if (currentHeader is not null)
                yield return new(currentHeader, [.. currentItems]);
        }

        public static IEnumerable<T> operator +(IEnumerable<T> a, IEnumerable<T> b)
            => a.Concat(b);

        public static IEnumerable<T> operator +(T a, IEnumerable<T> b)
            => b.Prepend(a);

        public static IEnumerable<T> operator +(IEnumerable<T> a, T b)
            => a.Append(b);

        public TR[] ToArray<TR>(Func<T, TR> selector)
            => source.Select(selector).ToArray();

        public List<TR> ToList<TR>(Func<T, TR> selector)
            => source.Select(selector).ToList();

        public TR[] ToArrayMany<TR>(Func<T, IEnumerable<TR>> selector)
            => source.SelectMany(selector).ToArray();

        public TR[] ToArray<TR>(Func<T, int, TR> selector)
            => source.Select(selector).ToArray();

        public bool ContainsAll(params IEnumerable<T> items)
            => items.All(source.Contains);

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

        public IEnumerable<T> Debug()
        {
            foreach (var item in source)
            {
                Console.WriteLine(item);
                yield return item;
            }
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
            => source.Concat(items);

        public IEnumerable<IEnumerable<T>> SplitBy(Func<T, bool> isHeader)
        {
            List<T>? list = null;

            foreach (var line in source)
            {
                if (isHeader(line))
                {
                    if (list is not null && list.Count != 0)
                        yield return list;

                    list = [];
                }

                list?.Add(line);
            }

            if (list is not null && list.Count != 0)
                yield return list;
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

        public TR SumMod<TR>(TR mod, Func<T, TR> func)
            where TR : INumber<TR>
            => source.Aggregate(TR.Zero, (agg, t) => Math.Mod(agg + func(t), mod));
    }

    //public static IEnumerable<T> Concat<T>(params IEnumerable<T>[] enumerables)
    //{
    //    foreach (var enumerable in enumerables)
    //        foreach (var item in enumerable)
    //            yield return item;
    //}

    //extension<T>(IReadOnlyList<T> collection)
    //{
    //    public IEnumerable<(T First, T Second)> EnumeratePairs()
    //    {
    //        for (var i = 0; i < collection.Count - 1; ++i)
    //        {
    //            for (var j = i + 1; j < collection.Count; ++j)
    //            {
    //                var a = collection[i];
    //                var b = collection[j];

    //                yield return (a, b);
    //            }
    //        }
    //    }
    //}

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
            const BindingFlags flags = BindingFlags.Static | BindingFlags.Public;
            var parameters = new[] { elementType };

            return enumerableType
                .GetMethod(nameof(Enumerable.ToArray), flags)!
                .MakeGenericMethod(parameters)
                .Invoke(null, [typeConvertedEnumerable])!;
        }

        object Cast(Type elementType)
        {
            var enumerableType = typeof(Enumerable);
            const BindingFlags flags = BindingFlags.Static | BindingFlags.Public;
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
            const BindingFlags flags = BindingFlags.Static | BindingFlags.Public;
            var parameters = new[] { elementType };

            return enumerableType
                .GetMethod(nameof(Enumerable.ToList), flags)!
                .MakeGenericMethod(parameters)
                .Invoke(null, [typeConvertedEnumerable])!;
        }
    }

    extension<T>(IEnumerable<T> source)
        where T : struct
    {
        //public IEnumerable<TResult> SelectPrevCurrNext<TResult>(Func<(T? Prev, T Curr, T? Next), TResult> selector)
        //{
        //    using var e = source.GetEnumerator();
        //    if (!e.MoveNext())
        //        yield break;

        //    var prev = default(T);
        //    var curr = e.Current;

        //    while (e.MoveNext())
        //    {
        //        var next = e.Current;
        //        yield return selector((prev, curr, next));
        //        prev = curr;
        //        curr = next;
        //    }

        //    yield return selector((prev, curr, default));
        //}

        public T? FirstOrNull()
            => source.Cast<T?>().FirstOrDefault();
    }

    //public static IEnumerable<Pos> Range2d(int n1, int n2)
    //{
    //    for (var i1 = 0; i1 < n1; ++i1)
    //        for (var i2 = 0; i2 < n2; ++i2)
    //            yield return new(i1, i2);
    //}

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
            throw new("No data");

        value = enumerator.Current;
    }

    extension<T>(IEnumerable<T> source)
        where T : INumber<T>
    {
        public T Sum()
            => source.Aggregate(T.Zero, (agg, t) => checked(agg + t));

        public T Mul()
            => source.Aggregate(T.One, (agg, t) => checked(agg * t));

        public long MulLong()
            => source.Aggregate(long.CreateChecked(T.One), (agg, t) => checked(agg * long.CreateChecked(t)));
    }
}

public record HeaderGrouping<TKey, TValue>(TKey Key, TValue[] Values);
