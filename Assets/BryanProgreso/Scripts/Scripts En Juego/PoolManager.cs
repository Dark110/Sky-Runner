using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager _instance;
    public static PoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("PoolManager");
                _instance = go.AddComponent<PoolManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    private Dictionary<GameObject, Queue<GameObject>> pools = new Dictionary<GameObject, Queue<GameObject>>();

    public GameObject GetFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(prefab))
            pools[prefab] = new Queue<GameObject>();

        GameObject obj;
        if (pools[prefab].Count > 0)
        {
            obj = pools[prefab].Dequeue();
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(prefab);
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        return obj;
    }

    public void ReturnToPool(GameObject prefab, GameObject obj)
    {
        obj.SetActive(false);
        if (!pools.ContainsKey(prefab))
            pools[prefab] = new Queue<GameObject>();

        pools[prefab].Enqueue(obj);
    }
}
