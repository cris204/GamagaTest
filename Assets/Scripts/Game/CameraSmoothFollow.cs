using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float damping;

    private Vector3 velocity;

    private void FixedUpdate()
    {
        Vector3 movePos = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, movePos, ref velocity, damping);
    }

    public void MoveInstant()
    {
        transform.position = target.position + offset;
    }

}
