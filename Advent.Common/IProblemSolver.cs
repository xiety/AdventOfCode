namespace Advent.Common;

public interface IProblemSolver<T>
{
    public T RunA(string filename);
    public T RunB(string filename) => throw new NotImplementedException();
}
