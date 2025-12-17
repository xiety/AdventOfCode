using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Numerics;
using System.Reflection;

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

        public bool TryMapTo<T>(string text, [NotNullWhen(true)] out T? result)
            => regex.Match(text).TryMapTo(regex, text, out result);
    }

    extension(Match match)
    {
        public T MapTo<T>()
        {
            if (!match.Success)
                throw new ArgumentOutOfRangeException(nameof(match), match, message: "Fail");

            var type = typeof(T);

            if (type == typeof(string) || type == typeof(int))
                return (T)Convert.ChangeType(match.Groups[1].Value, type);

            var constructor = type.GetConstructors()[0];
            var parameters = constructor.GetParameters();

            var comparer = StringComparer.OrdinalIgnoreCase;
            var groups = new Dictionary<string, Group>(match.Groups, comparer);

            if (!parameters.All(a => groups.ContainsKey(a.Name!)))
            {
                var wrong = parameters.Where(a => !groups.ContainsKey(a.Name!)).ToArray(a => a.Name);
                throw new ArgumentOutOfRangeException(paramName: nameof(match), message: $"Regex groups not found: {String.Join(", ", wrong)}");
            }

            var parametersValues = parameters.ToArray(a => ParseParameter(groups[a.Name!], a));

            var obj = (T)constructor.Invoke(parametersValues);

            if (obj is IInitializable init)
                init.Initialize();

            return obj;
        }

        public T MapTo<T>(Regex debugRegex, string debugText)
        {
            return !match.Success ? throw new ArgumentOutOfRangeException(nameof(match), match, message: $"Match failed on '{debugText}' with '{debugRegex}") : match.MapTo<T>();
        }

        public bool TryMapTo<T>(Regex debugRegex, string debugText, [NotNullWhen(true)] out T? result)
        {
            if (!match.Success)
            {
                result = default;
                return false;
            }

            result = match.MapTo<T>()!; //bang
            return true;
        }
    }

    static object ParseParameter(Group group, ParameterInfo pi)
    {
        var type = pi.ParameterType;

        var attr = pi.GetCustomAttribute(typeof(RegexParserAttribute<,>));

        if (attr is not null)
        {
            dynamic attrDynamic = new ReflectionDynamic(attr);
            dynamic parserDynamic = attrDynamic.CreateParser();
            return parserDynamic.Parse(group.Value);
        }

        if (type.IsArray)
        {
            var elementType = type.GetElementType()!;

            return group.Captures
                .Select(a => ParseValue(a.Value, elementType))
                .ToArray(elementType);
        }

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
        {
            var elementType = type.GetGenericArguments()[0];

            return group.Captures
                .Select(a => ParseValue(a.Value, elementType))
                .ToList(elementType);
        }
        
        return ParseValue(group.Value, type);
    }

    static object ParseValue(string value, Type type)
    {
        if (type == typeof(BigInteger))
            return BigInteger.Parse(value);
        return type.IsEnum
            ? Enum.Parse(type, value, true)
            : Convert.ChangeType(value, type);
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

            return m2.Success ? m2.MapTo<T2>(mr2, text)
                : throw new ArgumentOutOfRangeException($"Can not parse: {text}");
        }
    }
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public sealed class RegexParserAttribute<T, TR> : Attribute
    where T : IRegexParser<TR>, new()
{
    public T CreateParser()
        => new();
}

public interface IRegexParser<out T>
{
    T Parse(string text);
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class MapToAttribute<T> : Attribute;
