using System.Collections.Generic;
using UnityEngine;

public class XPGemPool : MonoBehaviour
{
    public static XPGemPool Instance { get; private set; }

    [System.Serializable]
    public class PoolConfig
    {
        public GameObject prefab;
        public int initialSize = 30;
    }

    public PoolConfig[] pools;

    private Dictionary<GameObject, Queue<GameObject>> queueByPrefab;
    private Dictionary<GameObject, GameObject> prefabByInstance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        queueByPrefab = new Dictionary<GameObject, Queue<GameObject>>(pools.Length);
        prefabByInstance = new Dictionary<GameObject, GameObject>(256);

        for (int poolIndex = 0; poolIndex < pools.Length; poolIndex++)
        {
            PoolConfig poolConfig = pools[poolIndex];
            Queue<GameObject> queue = new Queue<GameObject>(poolConfig.initialSize);

            for (int instanceIndex = 0; instanceIndex < poolConfig.initialSize; instanceIndex++)
            {
                GameObject instance = Instantiate(poolConfig.prefab, transform);
                instance.SetActive(false);
                queue.Enqueue(instance);
                prefabByInstance[instance] = poolConfig.prefab;
            }

            queueByPrefab[poolConfig.prefab] = queue;
        }
    }

    public GameObject Get(GameObject prefab, Vector3 position)
    {
        Queue<GameObject> queue;
        if (!queueByPrefab.TryGetValue(prefab, out queue))
        {
            queue = new Queue<GameObject>();
            queueByPrefab[prefab] = queue;
        }

        GameObject instance;
        if (queue.Count > 0)
        {
            instance = queue.Dequeue();
        }
        else
        {
            instance = Instantiate(prefab, transform);
            prefabByInstance[instance] = prefab;
        }

        instance.transform.position = position;
        instance.SetActive(true);
        return instance;
    }

    public void Release(GameObject instance)
    {
        if (!instance.activeSelf) return;
        instance.SetActive(false);

        GameObject prefab;
        if (prefabByInstance.TryGetValue(instance, out prefab))
        {
            queueByPrefab[prefab].Enqueue(instance);
        }
    }
}
