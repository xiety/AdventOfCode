using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Collections.Generic;

public static class DictionaryExtensions
{
    extension<TK, TV>(Dictionary<TK, TV> dic)
        where TK : notnull
        where TV : notnull
    {
        public Dictionary<TV, TK> Invert()
            => dic.ToDictionary(x => x.Value, x => x.Key);
    }

    extension<TK, TV>(Dictionary<TK, TV> dic)
        where TK : notnull
    {
        public void Replace(TK key, Func<TV, TV> replaceFunc)
        {
            ref var valueRef = ref CollectionsMarshal.GetValueRefOrNullRef(dic, key);

            if (Unsafe.IsNullRef(ref valueRef))
                throw new ArgumentOutOfRangeException(paramName: nameof(key), actualValue: key, message: "Key not found");

            valueRef = replaceFunc(valueRef);
        }

        public void AddOrReplace(TK key, TV initialValue, Func<TV, TV> replaceFunc)
        {
            ref var valueRef = ref CollectionsMarshal.GetValueRefOrNullRef(dic, key);

            if (Unsafe.IsNullRef(ref valueRef))
                dic.Add(key, initialValue);
            else
                valueRef = replaceFunc(valueRef);
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
