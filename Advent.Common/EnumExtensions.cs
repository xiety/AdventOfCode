namespace System;

public static class EnumExtensions
{
    extension<T>(T currentValue) where T : Enum
    {
        public T RotateEnum(int step)
        {
            var enumLength = Enum.GetValues(typeof(T)).Length;
            var newIndex = ((int)(object)currentValue + step) % enumLength;

            if (newIndex < 0)
                newIndex += enumLength;

            return (T)Enum.ToObject(typeof(T), newIndex);
        }
    }
}
