using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour {
    [SerializeField]
    float Height;
    [SerializeField]
    float WorldMaxZ;
    [SerializeField]
    float WorldMinZ;
    [SerializeField]
    float ZOffset;
    [SerializeField]
    float PositionLerpSpeed;
    [SerializeField]
    float RotationLerpSpeed;
    [SerializeField]

    public Transform Target;

	void Update () {
        float desiredZ = Mathf.Clamp(Target.position.z + ZOffset, WorldMinZ, WorldMaxZ);
        Vector3 desiredPosition = new Vector3(Target.position.x, Height, desiredZ);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, PositionLerpSpeed * Time.deltaTime);
        Quaternion desiredRot = Quaternion.LookRotation(Target.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, RotationLerpSpeed * Time.deltaTime);
	}
}
