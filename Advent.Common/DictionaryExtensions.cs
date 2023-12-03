using System.Numerics;

namespace System.Collections.Generic;

public static class DictionaryExtensions
{
    public static void AddOrReplace<TK, TV>(this Dictionary<TK, TV> dic, TK key, TV initialValue, Func<TV, TV> replaceFunc)
        where TK : notnull
    {
        if (dic.TryGetValue(key, out var value))
        {
            dic[key] = replaceFunc(value);
        }
        else
        {
            dic.Add(key, initialValue);
        }
    }

    public static Dictionary<TK, TV> Merge<TK, TV>(this Dictionary<TK, TV> dic1, Dictionary<TK, TV> dic2)
        where TK : notnull
        where TV : INumber<TV>
    {
        return dic1.Concat(dic2).GroupBy(a => a.Key).ToDictionary(a => a.Key, a => a.Sum(b => b.Value));
    }
}
