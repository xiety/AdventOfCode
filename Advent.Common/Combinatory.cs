namespace Advent.Common;

public static class CombinatoryExtensions
{
    extension<T>(T[] source)
    {
        public IEnumerable<IEnumerable<T>> Permutations(int num)
        {
            if (num == 1)
                return source.Select(a => new[] { a });

            return source.Permutations(num - 1)
                .SelectMany(a => source.Where(b => !a.Contains(b)),
                            (a, b) => a.Append(b));
        }

        public IEnumerable<IEnumerable<T>> Combinations()
            => source.Combinations(source.Length);

        public IEnumerable<IEnumerable<T>> Combinations(int num)
        {
            return CombinationsRecurse(source, num);

            static IEnumerable<IEnumerable<T>> CombinationsRecurse(ArraySegment<T> source, int num)
            {
                if (num == 1)
                    return source.Select(a => new[] { a });

                return source
                    .Index()
                    .SelectMany(a => CombinationsRecurse(source.Slice(a.Index + 1), num - 1),
                                (a, b) => b.Prepend(a.Item));
            }
        }
    }
}
