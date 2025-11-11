using System.Numerics;

namespace System.Collections.Generic;

public static class DictionaryExtensions
{
    extension<TK, TV>(Dictionary<TK, TV> dic)
        where TK : notnull
    {
        public void AddOrReplace(TK key, TV initialValue, Func<TV, TV> replaceFunc)
        {
            if (dic.TryGetValue(key, out var value))
                dic[key] = replaceFunc(value);
            else
                dic.Add(key, initialValue);
        }

        public TV GetOrCreate(TK key, Func<TV> create)
        {
            if (!dic.TryGetValue(key, out var value))
            {
                value = create();
                dic.Add(key, value);
            }

            return value;
        }
    }

    extension<TK, TV>(Dictionary<TK, TV> dic1)
        where TK : notnull
        where TV : INumber<TV>
    {
        public Dictionary<TK, TV> Merge(Dictionary<TK, TV> dic2)
            => dic1
                .Concat(dic2)
                .GroupBy(a => a.Key)
                .ToDictionary(a => a.Key, a => a.Sum(b => b.Value));
    }
}
