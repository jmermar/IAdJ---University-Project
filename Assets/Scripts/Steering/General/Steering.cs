using UnityEngine;
using System.Collections;


[System.Serializable]
public class Steering
{
    public float angular;
    public Vector3 linear;

    public Steering()
    {
        angular = 0.0f;
        linear = Vector3.zero;
    }
}

[System.Serializable]
public struct Static
{
    public float orientation;
    public Vector3 position;

    public Static(Vector3 p, float o) {
        orientation = o;
        position = p;
    }

    public Matrix4x4 OrientationMatrix() {
        Matrix4x4 mat = Matrix4x4.identity;
        mat[0, 0] = Mathf.Cos(Mathf.Deg2Rad * orientation);
        mat[0, 2] = -Mathf.Sin(Mathf.Deg2Rad * orientation);
        mat[2, 0] = Mathf.Sin(Mathf.Deg2Rad * orientation);
        mat[2, 2] = Mathf.Cos(Mathf.Deg2Rad * orientation);
        return mat;
    }
}
