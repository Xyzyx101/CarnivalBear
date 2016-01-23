using UnityEngine;
using System.Collections;

public class MathUtils : MonoBehaviour
{
    // Project vector on plane
    static public Vector3 ProjectVectorOnPlane(Vector3 planeNormal, Vector3 vector)
    {
        return vector - (Vector3.Dot(vector, planeNormal) * planeNormal);
    }

    // Get signed vector angle
    static public float SignedVectorAngle(Vector3 referenceVector, Vector3 otherVector, Vector3 normal)
    {
        Vector3 perpVector;
        float angle;

        perpVector = Vector3.Cross(normal, referenceVector);
        angle = Vector3.Angle(referenceVector, otherVector);
        angle *= Mathf.Sign(Vector3.Dot(perpVector, otherVector));

        return angle;
    }
}



