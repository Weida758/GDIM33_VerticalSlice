using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance { get; private set; }

    [System.Serializable]
    public class PoolConfig
    {
        public GameObject prefab;
        public int initialSize = 50;
    }

    public PoolConfig[] pools;

    private Dictionary<GameObject, Queue<GameObject>> poolMap;
    private Dictionary<GameObject, GameObject> instanceToPrefab;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        poolMap = new Dictionary<GameObject, Queue<GameObject>>(pools.Length);
        instanceToPrefab = new Dictionary<GameObject, GameObject>(256);

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

    public GameObject Get(GameObject prefab, Vector2 position)
    {
        if (!poolMap.TryGetValue(prefab, out var queue))
        {
            queue = new Queue<GameObject>();
            poolMap[prefab] = queue;
        }

        GameObject obj;
        if (queue.Count > 0)
        {
            obj = queue.Dequeue();
        }
        else
        {
            obj = Instantiate(prefab, transform);
            instanceToPrefab[obj] = prefab;
        }

        obj.transform.position = position;
        obj.SetActive(true);
        return obj;
    }

    public void Release(GameObject obj)
    {
        if (!obj.activeSelf) return;
        obj.SetActive(false);

        if (instanceToPrefab.TryGetValue(obj, out var prefab))
            poolMap[prefab].Enqueue(obj);
    }
}