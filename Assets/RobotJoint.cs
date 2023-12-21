using UnityEngine;

public class RobotJoint : MonoBehaviour
{
    public Vector3 axis;
	[HideInInspector] public Vector3 StartOffset;
    [HideInInspector] public Quaternion StartRotation;

    public float MinAngle;
    public float MaxAngle;

    void Awake()
    {
        StartOffset = transform.localPosition;
        StartRotation = transform.localRotation;

	}
}
