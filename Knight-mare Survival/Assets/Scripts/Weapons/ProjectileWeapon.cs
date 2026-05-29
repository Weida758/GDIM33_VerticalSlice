using UnityEngine;

public class ProjectileWeapon : Weapon
{
    [SerializeField] private BouncingProjectile projectilePrefab;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Camera boundaryCamera;
    [SerializeField] private Transform projectileParent;
    [SerializeField] private Vector2 centerOffset = new Vector2(0f, 3f);
    [SerializeField] private float spawnDistance = 0.35f;
    [SerializeField] private bool randomizeVolleyRotation = true;

    private float cooldownTimer;

    protected override void OnInitialized()
    {
        if (boundaryCamera == null) boundaryCamera = Camera.main;
        cooldownTimer = 0f;
    }

    protected override void OnUpgraded()
    {
        cooldownTimer = Mathf.Min(cooldownTimer, GetCooldown());
    }

    private void Update()
    {
        if (Data == null || projectilePrefab == null) return;
        if (boundaryCamera == null) boundaryCamera = Camera.main;

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            FireVolley();
            cooldownTimer = GetCooldown();
        }
    }

    private void FireVolley()
    {
        WeaponLevel stats = Stats;

        int projectileCount = Mathf.Max(1, stats.count);
        float projectileRadius = Mathf.Max(0.01f, stats.area);
        float projectileSpeed = Mathf.Max(0.01f, stats.speed);
        float projectileLifetime = Mathf.Max(0.01f, stats.duration);
        Vector2 fireCenter = GetFireCenter();

        float angleStep = 360f / projectileCount;
        float baseAngle = randomizeVolleyRotation ? Random.Range(0f, 360f) : 0f;

        for (int i = 0; i < projectileCount; i++)
        {
            Vector2 direction = DirectionFromAngle(baseAngle + angleStep * i);
            Vector2 spawnPosition = fireCenter + direction * (spawnDistance + projectileRadius);

            BouncingProjectile projectile = Instantiate(
                projectilePrefab,
                spawnPosition,
                Quaternion.identity,
                projectileParent
            );

            projectile.Initialize(
                stats.damage,
                projectileSpeed,
                projectileRadius,
                projectileLifetime,
                direction,
                enemyLayer,
                boundaryCamera
            );
        }
    }

    private float GetCooldown()
    {
        return Mathf.Max(0.05f, Stats.cooldown);
    }

    private Vector2 GetFireCenter()
    {
        return (Vector2)transform.position + centerOffset;
    }

    private static Vector2 DirectionFromAngle(float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
    }
}
