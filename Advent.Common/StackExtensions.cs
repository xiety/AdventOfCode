namespace System.Collections.Generic;

public static class StackExtensions
{
    extension<T>(Stack<T> stack)
    {
        public IEnumerable<T> PopMultiple(int quantity)
        {
            for (var i = 0; i < quantity; ++i)
                yield return stack.Pop();
        }
    }
}
