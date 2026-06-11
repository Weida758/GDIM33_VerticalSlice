using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class EnemySpecialItemDrop : MonoBehaviour
{
    [System.Serializable]
    public class DropOption
    {
        public GameObject prefab;
        [Min(0f)] public float weight = 1f;
    }

    [SerializeField, Range(0f, 1f)] private float dropChance = 0.05f;
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField] private DropOption[] possibleDrops;

    private EnemyHealth health;

    private void Awake()
    {
        health = GetComponent<EnemyHealth>();
        health.OnDied += HandleDied;
    }

    private void OnDestroy()
    {
        if (health != null)
        {
            health.OnDied -= HandleDied;
        }
    }

    private void HandleDied(Vector3 deathPosition)
    {
        if (possibleDrops == null || possibleDrops.Length == 0) return;
        if (Random.value > dropChance) return;

        GameObject prefab = PickDropPrefab();
        if (prefab == null) return;

        Instantiate(prefab, deathPosition + spawnOffset, Quaternion.identity);
    }

    private GameObject PickDropPrefab()
    {
        float totalWeight = 0f;
        for (int i = 0; i < possibleDrops.Length; i++)
        {
            DropOption option = possibleDrops[i];
            if (option == null || option.prefab == null || option.weight <= 0f) continue;
            totalWeight += option.weight;
        }

        if (totalWeight <= 0f) return null;

        float roll = Random.Range(0f, totalWeight);
        for (int i = 0; i < possibleDrops.Length; i++)
        {
            DropOption option = possibleDrops[i];
            if (option == null || option.prefab == null || option.weight <= 0f) continue;

            if (roll < option.weight)
            {
                return option.prefab;
            }

            roll -= option.weight;
        }

        return null;
    }
}
