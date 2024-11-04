namespace System;

public static class EnumExtensions
{
    public static T RotateEnum<T>(this T currentValue, int step)
        where T : Enum
    {
        int enumLength = Enum.GetValues(typeof(T)).Length;
        int newIndex = ((int)(object)currentValue + step) % enumLength;

        if (newIndex < 0)
            newIndex += enumLength;

        return (T)Enum.ToObject(typeof(T), newIndex);
    }
}
