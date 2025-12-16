using System.Numerics;

namespace Advent.Common;

public static class RangeExtensions
{
    extension(Range range)
    {
        public IEnumerator<int> GetEnumerator()
        {
            if (range.Start.IsFromEnd || range.End.IsFromEnd)
                throw new NotSupportedException();

            var start = range.Start.Value;
            var end = range.End.Value;

            if (start < end)
            {
                for (var i = start; i < end; ++i)
                    yield return i;
            }
            else
            {
                for (var i = start; i > end; --i)
                    yield return i;
            }
        }
    }

    extension<T>(T number)
        where T : INumber<T>
    {
        public IEnumerator<T> GetEnumerator()
        {
            for (var i = T.Zero; i < number; i += T.One)
                yield return i;
        }
    }
}
