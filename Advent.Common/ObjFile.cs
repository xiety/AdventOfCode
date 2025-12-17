using System.Numerics;

namespace Advent.Common;

public class ObjFile
{
    static readonly Vector3[] CubeVertices = [
        new(0, 0, 0), // 0
        new(1, 0, 0), // 1
        new(1, 1, 0), // 2
        new(0, 1, 0), // 3
        new(0, 1, 1), // 4
        new(1, 1, 1), // 5
        new(1, 0, 1), // 6
        new(0, 0, 1),  // 7
    ];

    static readonly int[,] CubeFaces = new [,] {
        {0, 1, 2, 3}, // left
        {4, 5, 6, 7}, // right
        {0, 4, 7, 1}, // bottom
        {2, 3, 4, 5}, // top
        {0, 3, 4, 7}, // back
        {1, 2, 5, 6},  // front
    };

    readonly List<Vector3> vList = [];
    readonly List<int[]> fList = [];

    //mutable
    int offset = 1;

    public void AddCube(Vector3 pos1, Vector3 pos2)
    {
        var size = pos2 - pos1;

        for (var i = 0; i < 8; ++i)
            vList.Add(CubeVertices[i] * size + pos1);

        for (var i = 0; i < 6; i++)
            fList.Add([CubeFaces[i, 0] + offset, CubeFaces[i, 1] + offset, CubeFaces[i, 2] + offset, CubeFaces[i, 3] + offset]);

        offset += 8;
    }

    public void Save(string filename)
    {
        using var writer = new StreamWriter(filename);

        foreach (var v in vList)
            writer.WriteLine($"v {v[0]} {v[1]} {v[2]}");

        foreach (var f in fList)
            writer.WriteLine($"f {String.Join(" ", f)}");
    }
}
