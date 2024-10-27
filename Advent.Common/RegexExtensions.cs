using System.Numerics;

namespace System.Text.RegularExpressions;

public static class RegexExtensions
{
    public static List<T> FromFile<T>(this Regex regex, string filename)
        => regex.FromLines<T>(File.ReadAllLines(filename));

    public static List<T> FromLines<T>(this Regex regex, IEnumerable<string> lines)
        => lines.Select(regex.MapTo<T>).ToList();

    public static T MapTo<T>(this Match match, string debugText)
    {
        if (!match.Success)
            throw new ArgumentOutOfRangeException(nameof(match), match, message: $"Match failed on '{debugText}'");

        var type = typeof(T);
        var constructor = type.GetConstructors().First();
        var parameters = constructor.GetParameters();

        var comparer = StringComparer.OrdinalIgnoreCase;
        var groups = new Dictionary<string, Group>(match.Groups, comparer);

        if (!parameters.All(a => groups.ContainsKey(a.Name!)))
        {
            var wrong = parameters.Where(a => !groups.ContainsKey(a.Name!)).Select(a => a.Name).ToArray();
            throw new ArgumentOutOfRangeException(paramName: nameof(match), message: $"Regex groups not found: {String.Join(", ", wrong)}");
        }

        var parametersValues = parameters.Select(a => Parse(groups[a.Name!], a.ParameterType)).ToArray();

        var obj = (T)constructor.Invoke(parametersValues);

        if (obj is IInitializable init)
            init.Initialize();

        return obj;
    }

    private static object Parse(Group group, Type type)
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

    private static object Parse(string value, Type type)
    {
        if (type == typeof(BigInteger))
            return BigInteger.Parse(value);

        return Convert.ChangeType(value, type);
    }

    public static T MapTo<T>(this string text, Regex regex)
        => regex.Match(text).MapTo<T>(text);

    public static T MapTo<T>(this Regex regex, string text)
        => regex.Match(text).MapTo<T>(text);

    public static T MapTo<T, T1, T2>(this string text, Regex mr1, Regex mr2)
        where T1 : T
        where T2 : T
    {
        var m1 = mr1.Match(text);

        if (m1.Success)
            return m1.MapTo<T1>(text);

        var m2 = mr2.Match(text);

        if (m2.Success)
            return m2.MapTo<T2>(text);

        throw new ArgumentOutOfRangeException($"Can not parse: {text}");
    }
}
