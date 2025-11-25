using System.Runtime.CompilerServices;

namespace System;

public static class ConsoleExtensions
{
    extension(Console)
    {
        public static void Debug(object value, [CallerArgumentExpression(nameof(value))]string valueExpression = "")
            => Console.WriteLine($"{valueExpression}: {value}");

        public static void Debug<T>(IEnumerable<T> value, [CallerArgumentExpression(nameof(value))] string valueExpression = "")
        {
            Console.WriteLine($"{valueExpression}:");
            Console.WriteLine(value.StringJoin(Environment.NewLine));
            Console.WriteLine($"---");
        }
    }
}
