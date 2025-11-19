namespace Advent.Common;

public interface IProblemSolver<TR>
{
    TR RunA(string filename) => throw new NotImplementedException();
    TR RunB(string filename) => throw new NotImplementedException();
}

public interface IProblemSolver<TRA, TRB>
{
    TRA RunA(string filename) => throw new NotImplementedException();
    TRB RunB(string filename) => throw new NotImplementedException();
}

public interface ISolver<TR>
{
    TR RunA(string[] lines, bool isSample) => throw new NotImplementedException();
    TR RunB(string[] lines, bool isSample) => throw new NotImplementedException();
}

public interface ISolver<TRA, TRB>
{
    TRA RunA(string[] lines, bool isSample) => throw new NotImplementedException();
    TRB RunB(string[] lines, bool isSample) => throw new NotImplementedException();
}
