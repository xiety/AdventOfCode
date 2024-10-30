namespace Advent.Common;

public class ResizableArray<T>(T[] array)
{
    public int Length => array.Length;

    public T this[int p]
    {
        get
        {
            Resize(p);
            return array[p];
        }
        set
        {
            Resize(p);
            array[p] = value;
        }
    }

    void Resize(int size)
    {
        if (array.Length <= size)
            Array.Resize(ref array, size + 1);
    }
}
