using UnityEngine;

public class Sheep : MonoBehaviour
{
    [SerializeField] public Transform tpPoint;
    [SerializeField] public ChangeDialog dialog;
    [SerializeField] public RobotController robot;

    public void Teleportation()
    {
        transform.position = tpPoint.transform.position;
        transform.localScale = Vector3.one;
        dialog.TriggerTpEvent();
        robot.StopObject();
    }
}