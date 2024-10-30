﻿namespace Advent.Common;

public interface IProblemSolver<TR>
{
    public TR RunA(string filename) => throw new NotImplementedException();
    public TR RunB(string filename) => throw new NotImplementedException();
}

public interface IProblemSolver<TRA, TRB>
{
    public TRA RunA(string filename) => throw new NotImplementedException();
    public TRB RunB(string filename) => throw new NotImplementedException();
}

public interface ISolver<TR>
{
    public TR RunA(string input, bool isSample);
    public TR RunB(string input, bool isSample);
}
