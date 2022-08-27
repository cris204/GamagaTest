using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooleableObject : MonoBehaviour
{
    public string path;
    public void ReturnToPool()
    {
        if (this.gameObject.activeInHierarchy) {
            PoolManager.Instance.ReleaseObject(this.gameObject);
        }
    }
}