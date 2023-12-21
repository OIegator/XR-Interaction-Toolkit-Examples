using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    [SerializeField]
    public Transform tp_point;
    public void teleportation()
    {
        transform.position = tp_point.transform.position;
    }
}
