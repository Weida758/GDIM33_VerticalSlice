using UnityEngine;

public class EnemyProjectileShooter : MonoBehaviour
{
    [SerializeField] private EnemyProjectile projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float fireInterval = 2.5f;
    [SerializeField] private float firstShotDelay = 1f;
    [SerializeField] private float projectileSpeed = 7f;
    [SerializeField] private float projectileLifetime = 5f;
    [SerializeField] private int projectileDamage = 12;
    [SerializeField] private float maxFireDistance = 18f;

    private Transform playerTransform;
    private float fireTimer;
    private int baseProjectileDamage;

    private void Awake()
    {
        baseProjectileDamage = projectileDamage;
    }

    private void OnEnable()
    {
        projectileDamage = baseProjectileDamage;
        fireTimer = firstShotDelay;
        ResolvePlayer();
    }

    public void ApplyDamageMultiplier(float multiplier)
    {
        projectileDamage = Mathf.Max(1, Mathf.RoundToInt(baseProjectileDamage * Mathf.Max(0.01f, multiplier)));
    }

    private void Update()
    {
        if (projectilePrefab == null) return;

        if (playerTransform == null)
        {
            ResolvePlayer();
            if (playerTransform == null) return;
        }

        fireTimer -= Time.deltaTime;
        if (fireTimer > 0f) return;

        Vector2 toPlayer = playerTransform.position - transform.position;
        if (toPlayer.sqrMagnitude <= maxFireDistance * maxFireDistance)
        {
            Fire(toPlayer);
        }

        fireTimer = fireInterval;
    }

    private void ResolvePlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
    }

    private void Fire(Vector2 toPlayer)
    {
        Transform spawnPoint = projectileSpawnPoint != null ? projectileSpawnPoint : transform;
        EnemyProjectile projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        projectile.Initialize(toPlayer, projectileDamage, projectileSpeed, projectileLifetime);
    }
}
