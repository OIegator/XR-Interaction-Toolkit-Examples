using UnityEngine;

public class Battery : MonoBehaviour
{
    [SerializeField] public Transform tpPoint;

    public void Teleportation()
    {
        transform.position = tpPoint.transform.position;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}