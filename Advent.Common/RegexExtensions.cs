using System.Numerics;

namespace System.Text.RegularExpressions;

public static class RegexExtensions
{
    extension(Regex regex)
    {
        public T[] FromFile<T>(string filename)
        => regex.FromLines<T>(File.ReadAllLines(filename));

        public T[] FromLines<T>(IEnumerable<string> lines)
            => lines.ToArray(regex.MapTo<T>);

        public T MapTo<T>(string text)
            => regex.Match(text).MapTo<T>(regex, text);
    }

    extension(Match match)
    {
        public T MapTo<T>()
        {
            if (!match.Success)
                throw new ArgumentOutOfRangeException(nameof(match), match, message: "Fail");

            var type = typeof(T);
            var constructor = type.GetConstructors().First();
            var parameters = constructor.GetParameters();

            var comparer = StringComparer.OrdinalIgnoreCase;
            var groups = new Dictionary<string, Group>(match.Groups, comparer);

            if (!parameters.All(a => groups.ContainsKey(a.Name!)))
            {
                var wrong = parameters.Where(a => !groups.ContainsKey(a.Name!)).ToArray(a => a.Name);
                throw new ArgumentOutOfRangeException(paramName: nameof(match), message: $"Regex groups not found: {String.Join(", ", wrong)}");
            }

            var parametersValues = parameters.ToArray(a => Parse(groups[a.Name!], a.ParameterType));

            var obj = (T)constructor.Invoke(parametersValues);

            if (obj is IInitializable init)
                init.Initialize();

            return obj;
        }

        public T MapTo<T>(Regex debugRegex, string debugText)
        {
            if (!match.Success)
                throw new ArgumentOutOfRangeException(nameof(match), match, message: $"Match failed on '{debugText}' with '{debugRegex}");

            return match.MapTo<T>();
        }
    }

    static object Parse(Group group, Type type)
    {
        if (type.IsArray)
        {
            var elementType = type.GetElementType()!;

            return group.Captures
                .Select(a => Parse(a.Value, elementType))
                .ToArray(elementType);
        }
        else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
        {
            var elementType = type.GetGenericArguments().First();

            return group.Captures
                .Select(a => Parse(a.Value, elementType))
                .ToList(elementType);
        }
        else
        {
            return Parse(group.Value, type);
        }
    }

    static object Parse(string value, Type type)
    {
        if (type == typeof(BigInteger))
            return BigInteger.Parse(value);

        return Convert.ChangeType(value, type);
    }

    extension(string text)
    {
        public T MapTo<T>(Regex regex)
        => regex.Match(text).MapTo<T>(regex, text);

        public T MapTo<T, T1, T2>(Regex mr1, Regex mr2)
            where T1 : T
            where T2 : T
        {
            var m1 = mr1.Match(text);

            if (m1.Success)
                return m1.MapTo<T1>(mr1, text);

            var m2 = mr2.Match(text);

            if (m2.Success)
                return m2.MapTo<T2>(mr2, text);

            throw new ArgumentOutOfRangeException($"Can not parse: {text}");
        }
    }
}
