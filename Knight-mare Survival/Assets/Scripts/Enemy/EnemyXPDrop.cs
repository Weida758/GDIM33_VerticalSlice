using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class EnemyXPDrop : MonoBehaviour
{
    [SerializeField] private GameObject gemPrefab;
    [SerializeField, Range(0f, 1f)] private float dropChance = 1f;
    [SerializeField, Min(1)] private int dropCount = 1;
    [SerializeField] private float dropSpreadRadius = 0.4f;
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

        for (int i = 0; i < dropCount; i++)
        {
            XPGemPool.Instance.Get(gemPrefab, spawnPosition + GetDropOffset());
        }
    }

    private Vector3 GetDropOffset()
    {
        if (dropSpreadRadius <= 0f) return Vector3.zero;

        float angle = Random.Range(0f, Mathf.PI * 2f);
        float radius = Random.Range(0f, dropSpreadRadius);

        return new Vector3(
            Mathf.Cos(angle) * radius,
            Mathf.Sin(angle) * radius,
            0f
        );
    }
}
