#pragma warning disable CA1815 // Override equals and operator equals on value types

namespace Advent.Common;

public static class TreeExtensions
{
    extension<T>(IEnumerable<T> source)
    {
        public RecursiveGraphBuilder<TKey, T> ToRecursiveGraphBuilder<TKey>(Func<T, TKey> keySelector)
            where TKey : notnull
            => new(source.ToDictionary(keySelector));
    }

    public readonly struct RecursiveGraphBuilder<TKey, TValue>(IDictionary<TKey, TValue> source)
        where TKey : notnull
    {
        public Dictionary<TKey, TNode> Build<TNode>(Func<TValue, Func<TKey, TNode>, TNode> factory)
        {
            var cache = new Dictionary<TKey, TNode>();

            var sourceLocal = source;

            foreach (var key in sourceLocal.Keys)
                Resolve(key);

            return cache;

            TNode Resolve(TKey key)
                => cache.GetOrCreate(key, () => factory(sourceLocal[key], Resolve));
        }
    }
}
