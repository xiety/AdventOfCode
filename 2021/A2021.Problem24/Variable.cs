namespace A2021.Problem24;

public class Variable
{
    readonly int[] values;

    public Variable()
    {
        values = Enumerable.Range(1, 9).ToArray();
    }

    public Variable(int[] values)
    {
        this.values = values;
    }

    public Variable Mul(Variable var)
    {
        return new((
            from a in values
            from b in var.values
            select a * b
        ).Distinct().ToArray());
    }

    public Variable Add(Variable var)
    {
        return new((
            from a in values
            from b in var.values
            select a + b
        ).Distinct().ToArray());
    }

    public Variable Div(Variable var)
    {
        return new((
            from a in values
            from b in var.values
            select a / b
        ).Distinct().ToArray());
    }

    public Variable Mod(Variable var)
    {
        return new((
            from a in values
            from b in var.values
            select a % b
        ).Distinct().ToArray());
    }
}
