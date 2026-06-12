using System.Collections.Generic;
using UnityEngine;

public class BouncingProjectile : MonoBehaviour
{
    [SerializeField] private float visualRadiusAtScaleOne = 0.25f;
    [SerializeField] private float visualScaleMultiplier = 1f;
    [SerializeField] private float enemyHitCooldown = 0.25f;
    [SerializeField] private int sortingOrder = 20;
    [SerializeField] private bool rotateToMovement = true;
    [SerializeField] private float spinDegreesPerSecond = 180f;

    private readonly Collider2D[] hitBuffer = new Collider2D[128];
    private readonly Dictionary<EnemyHealth, float> nextAllowedHitTime = new();

    private Vector3 baseLocalScale;
    private SpriteRenderer spriteRenderer;
    private Transform visualTransform;
    private Camera boundaryCamera;
    private LayerMask enemyLayer;
    private Vector2 direction;
    private float speed;
    private float radius;
    private float lifetimeRemaining;
    private float damage;
    private float spinDirection = 1f;
    private bool initialized;

    private void Awake()
    {
        baseLocalScale = transform.localScale;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        visualTransform = spriteRenderer != null ? spriteRenderer.transform : transform;
    }

    public void Initialize(
        float damage,
        float speed,
        float radius,
        float lifetime,
        Vector2 direction,
        LayerMask enemyLayer,
        Camera boundaryCamera)
    {
        this.damage = damage;
        this.speed = Mathf.Max(0f, speed);
        this.radius = Mathf.Max(0.01f, radius);
        this.lifetimeRemaining = Mathf.Max(0.01f, lifetime);
        this.direction = direction.sqrMagnitude > 0.0001f ? direction.normalized : Vector2.right;
        this.enemyLayer = enemyLayer;
        this.boundaryCamera = boundaryCamera;

        nextAllowedHitTime.Clear();
        initialized = true;
        spinDirection = Random.value < 0.5f ? -1f : 1f;
        if (spriteRenderer != null) spriteRenderer.sortingOrder = sortingOrder;

        float scale = this.radius / Mathf.Max(0.0001f, visualRadiusAtScaleOne);
        transform.localScale = baseLocalScale * scale * visualScaleMultiplier;

        ApplyRotation();
    }

    private void Update()
    {
        if (!initialized) return;

        lifetimeRemaining -= Time.deltaTime;
        if (lifetimeRemaining <= 0f)
        {
            Finish();
            return;
        }

        if (!Move(Time.deltaTime)) return;
        ApplySpin(Time.deltaTime);
        DamageEnemies();
    }

    private bool Move(float deltaTime)
    {
        Vector2 position = transform.position;
        position += direction * speed * deltaTime;

        if (boundaryCamera != null)
        {
            Rect bounds = GetCameraBounds();
            bool bounced = false;

            if (position.x < bounds.xMin + radius)
            {
                position.x = bounds.xMin + radius;
                direction.x = Mathf.Abs(direction.x);
                bounced = true;
            }
            else if (position.x > bounds.xMax - radius)
            {
                position.x = bounds.xMax - radius;
                direction.x = -Mathf.Abs(direction.x);
                bounced = true;
            }

            if (position.y < bounds.yMin + radius)
            {
                position.y = bounds.yMin + radius;
                direction.y = Mathf.Abs(direction.y);
                bounced = true;
            }
            else if (position.y > bounds.yMax - radius)
            {
                position.y = bounds.yMax - radius;
                direction.y = -Mathf.Abs(direction.y);
                bounced = true;
            }

            if (bounced)
            {
                ApplyRotation();
            }
        }

        transform.position = position;
        return true;
    }

    private void DamageEnemies()
    {
        int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, radius, hitBuffer, enemyLayer);
        float now = Time.time;

        for (int i = 0; i < hitCount; i++)
        {
            EnemyHealth enemy = hitBuffer[i].GetComponentInParent<EnemyHealth>();
            if (enemy == null) enemy = hitBuffer[i].GetComponent<EnemyHealth>();
            if (enemy == null) continue;

            if (nextAllowedHitTime.TryGetValue(enemy, out float nextTime) && now < nextTime)
                continue;

            enemy.TakeDamage(damage, transform.position);
            nextAllowedHitTime[enemy] = now + enemyHitCooldown;
        }
    }

    private Rect GetCameraBounds()
    {
        float depth = Mathf.Abs(boundaryCamera.transform.position.z - transform.position.z);
        Vector3 min = boundaryCamera.ViewportToWorldPoint(new Vector3(0f, 0f, depth));
        Vector3 max = boundaryCamera.ViewportToWorldPoint(new Vector3(1f, 1f, depth));

        return Rect.MinMaxRect(
            Mathf.Min(min.x, max.x),
            Mathf.Min(min.y, max.y),
            Mathf.Max(min.x, max.x),
            Mathf.Max(min.y, max.y)
        );
    }

    private void ApplyRotation()
    {
        if (!rotateToMovement) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void ApplySpin(float deltaTime)
    {
        if (visualTransform == null || spinDegreesPerSecond == 0f) return;

        visualTransform.Rotate(0f, 0f, spinDegreesPerSecond * spinDirection * deltaTime, Space.Self);
    }

    private void Finish()
    {
        initialized = false;
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        float drawRadius = initialized ? radius : visualRadiusAtScaleOne;
        Gizmos.DrawWireSphere(transform.position, drawRadius);
    }
}
