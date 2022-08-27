using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager instance;
    public static PoolManager Instance
    {
        get
        {
            return instance;
        }
    }

    public Dictionary<string, List<GameObject>> objectsPool = new Dictionary<string, List<GameObject>>();

    private void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this);
        }
    }


    public GameObject GetObject(string path)
    {
        if (!objectsPool.ContainsKey(path)) {
            objectsPool.Add(path, new List<GameObject>());
        }

        if (objectsPool[path].Count == 0) {
            InstantiateObject(path);
        }

        return AllocateObject(path);
    }

    public void ReleaseObject(GameObject prefab)
    {
        string path = prefab.GetComponent<PooleableObject>().path;
        ReleaseObject(path, prefab);
    }

    public void ReleaseObject(string path, GameObject prefab)
    {
        PooleableObject prefabPooleable = prefab.GetComponent<PooleableObject>();
        if (prefabPooleable == null) {
            prefab.AddComponent<PooleableObject>();
            prefabPooleable = prefab.GetComponent<PooleableObject>();
            prefabPooleable.path = path;
        }
        prefab.gameObject.SetActive(false);

        if (!objectsPool.ContainsKey(path)) {
            objectsPool.Add(path, new List<GameObject>());
        }
        prefab.transform.SetParent(transform);
        objectsPool[path].Add(prefab);
    }

    private void InstantiateObject(string path)
    {
        if (ResourcesManager.Instance.GetGameObject(path) != null) {
            GameObject instance = Instantiate(ResourcesManager.Instance.GetGameObject(path), transform);
            if (instance != null) {
                PooleableObject poolable = instance.GetComponent<PooleableObject>();
                if (poolable == null) {
                    poolable = instance.AddComponent<PooleableObject>();
                }
                poolable.path = path;
                instance.gameObject.SetActive(false);
                instance.transform.position = transform.position;
                objectsPool[path].Add(instance);
            }
        } else {
            Debug.LogWarning("object in path " + path + " doesnt exist");
        }
    }

    private GameObject AllocateObject(string path)
    {
        if (objectsPool[path] != null && objectsPool[path].Count > 0) {
            GameObject objectPool = objectsPool[path][0];
            objectsPool[path].RemoveAt(0);
            objectPool.gameObject.SetActive(true);
            return objectPool;
        } else {
            Debug.LogWarning("item " + path + " no found");
            return null;
        }
    }
    public T Get<T>(string path) where T : UnityEngine.Object
    {
        GameObject obj = GetObject(path);
        if (obj != null) {
            return obj.GetComponent<T>();
        } else {
            return null;
        }
    }

    public bool ExistsAtLeastOneItemByType(string path)
    {
        if (objectsPool.ContainsKey(path) && objectsPool[path].Count > 0) {
            return true;
        }

        return false;
    }

}