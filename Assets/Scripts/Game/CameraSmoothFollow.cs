using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float damping;

    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;

    private Vector3 cameraPos;
    private Vector3 velocity;

    public void SetUpCamera(float inMinX, float inMaxX, float inMinZ, float inMaxZ)
    {
        minX = inMinX;
        maxX = inMaxX;
        
        minZ = inMinZ;
        maxZ = inMaxZ;
    }

    private void FixedUpdate()
    {
        Vector3 movePos = target.position + offset;

        movePos.x = Mathf.Clamp(movePos.x, minX, maxX);
        movePos.z = Mathf.Clamp(movePos.z, minZ, maxZ);

        transform.position = Vector3.SmoothDamp(transform.position, movePos, ref velocity, damping);
    }

    public void MoveInstant()
    {
        Vector3 movePos = target.position + offset;

        movePos.x = Mathf.Clamp(movePos.x, minX, maxX);
        movePos.z = Mathf.Clamp(movePos.z, minZ, maxZ);

        transform.position = movePos;
    }

}
