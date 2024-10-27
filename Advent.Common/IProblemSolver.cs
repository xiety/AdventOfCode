namespace Advent.Common;

public interface IProblemSolver<out TR>
{
    public TR RunA(string filename);
    public TR RunB(string filename) => throw new NotImplementedException();
}

public interface IProblemSolver<out TRA, out TRB>
{
    public TRA RunA(string filename);
    public TRB RunB(string filename) => throw new NotImplementedException();
}
