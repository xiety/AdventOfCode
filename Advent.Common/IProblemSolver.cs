namespace Advent.Common;

public interface IProblemSolver<out TR>
{
    TR RunA(string filename) => throw new NotImplementedException();
    TR RunB(string filename) => throw new NotImplementedException();
}

public interface IProblemSolver<out TRA, out TRB>
{
    TRA RunA(string filename) => throw new NotImplementedException();
    TRB RunB(string filename) => throw new NotImplementedException();
}

public interface ISolver<out TR>
{
    TR RunA(string[] lines, bool isSample) => throw new NotImplementedException();
    TR RunB(string[] lines, bool isSample) => throw new NotImplementedException();
}

public interface ISolver<out TRA, out TRB>
{
    TRA RunA(string[] lines, bool isSample) => throw new NotImplementedException();
    TRB RunB(string[] lines, bool isSample) => throw new NotImplementedException();
}
