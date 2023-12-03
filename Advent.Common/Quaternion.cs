using System.Numerics;

namespace Numerics;

public static class QuaternionHelper
{
    public static Quaternion Rotation(Vector3 from, Vector3 to)
    {
        var dot = Vector3.Dot(from, to);
        var cross = Vector3.Cross(from, to);

        var q = new Quaternion(cross.X, cross.Y, cross.Z, 1 + dot);

        return Quaternion.Normalize(q);
    }
}
