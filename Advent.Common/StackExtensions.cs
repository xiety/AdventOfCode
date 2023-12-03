namespace System.Collections.Generic;

public static class StackExtensions
{
    public static IEnumerable<T> PopMultiple<T>(this Stack<T> stack, int quantity)
    {
        for (var i = 0; i < quantity; ++i)
            yield return stack.Pop();
    }
}
