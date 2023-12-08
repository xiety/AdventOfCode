namespace Advent.Common;

public interface IProblemSolver<TR>
{
    public TR RunA(string filename);
    public TR RunB(string filename) => throw new NotImplementedException();
}

public interface IProblemSolver<TRA, TRB>
{
    public TRA RunA(string filename);
    public TRB RunB(string filename) => throw new NotImplementedException();
}
