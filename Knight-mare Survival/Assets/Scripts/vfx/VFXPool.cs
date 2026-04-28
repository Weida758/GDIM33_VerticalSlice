using System.Collections.Generic;
using UnityEngine;

public class VFXPool : MonoBehaviour
{
    public static VFXPool Instance { get; private set; }

    [System.Serializable]
    public class PoolConfig
    {
        public GameObject prefab;
        public int initialSize = 16;
    }

    public PoolConfig[] pools;

    private Dictionary<GameObject, Queue<GameObject>> poolMap;
    private Dictionary<GameObject, GameObject> instanceToPrefab;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        poolMap = new Dictionary<GameObject, Queue<GameObject>>(pools.Length);
        instanceToPrefab = new Dictionary<GameObject, GameObject>(128);

        for (int i = 0; i < pools.Length; i++)
        {
            var queue = new Queue<GameObject>(pools[i].initialSize);
            for (int j = 0; j < pools[i].initialSize; j++)
            {
                var obj = Instantiate(pools[i].prefab, transform);
                obj.SetActive(false);
                queue.Enqueue(obj);
                instanceToPrefab[obj] = pools[i].prefab;
            }
            poolMap[pools[i].prefab] = queue;
        }
    }

    public GameObject Get(GameObject prefab, Vector2 position, Quaternion rotation, Transform parent = null)
    {
        if (!poolMap.TryGetValue(prefab, out var queue))
        {
            queue = new Queue<GameObject>();
            poolMap[prefab] = queue;
        }

        GameObject obj;
        if (queue.Count > 0)
            obj = queue.Dequeue();
        else
        {
            obj = Instantiate(prefab, transform);
            instanceToPrefab[obj] = prefab;
        }

        var t = obj.transform;
        t.SetParent(parent != null ? parent : transform, true);
        t.position = position;
        t.rotation = rotation;
        obj.SetActive(true);
        return obj;
    }

    public void Release(GameObject obj)
    {
        if (!obj.activeSelf) return;
        obj.SetActive(false);
        obj.transform.SetParent(transform, true);

        if (instanceToPrefab.TryGetValue(obj, out var prefab))
            poolMap[prefab].Enqueue(obj);
    }
}
