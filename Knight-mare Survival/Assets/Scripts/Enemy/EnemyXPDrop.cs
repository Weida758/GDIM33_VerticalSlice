using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class EnemyXPDrop : MonoBehaviour
{
    [SerializeField] private GameObject gemPrefab;
    [SerializeField, Range(0f, 1f)] private float dropChance = 1f;
    [SerializeField] private Vector3 spawnOffset;

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
        if (gemPrefab == null) return;
        if (XPGemPool.Instance == null) return;

        if (dropChance < 1f && Random.value > dropChance) return;

        Vector3 spawnPosition = deathPosition + spawnOffset;
        XPGemPool.Instance.Get(gemPrefab, spawnPosition);
    }
}
