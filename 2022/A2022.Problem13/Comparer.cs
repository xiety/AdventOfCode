namespace A2022.Problem13;

class Comparer
{
    public static int Compare(Item first, Item second)
    {
        {
            if (first is ItemValue f && second is ItemValue s)
                return Compare(f.Value, s.Value);
        }

        {
            if (first is ItemValue f && second is ItemArray s)
                return Compare(new ItemArray([f]), s);
        }

        {
            if (first is ItemArray f && second is ItemValue s)
                return Compare(f, new ItemArray([s]));
        }

        {
            if (first is ItemArray f && second is ItemArray s)
            {
                var max = Math.Max(f.Items.Length, s.Items.Length);

                for (var i = 0; i < max; ++i)
                {
                    if (i < f.Items.Length && i < s.Items.Length)
                    {
                        var c = Compare(f.Items[i], s.Items[i]);

                        if (c != 0)
                            return c;
                    }
                    else
                    {
                        return Compare(f.Items.Length, s.Items.Length);
                    }
                }
            }

            return 0;
        }
    }

    private static int Compare<T>(T a, T b)
        => Comparer<T>.Default.Compare(a, b);
}
