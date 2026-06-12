using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float speed = 6f;
    [SerializeField] private float lifetime = 4f;

    private Vector2 direction = Vector2.right;
    private float lifetimeTimer;

    public void Initialize(Vector2 direction, int damage, float speed, float lifetime)
    {
        this.direction = direction.sqrMagnitude > 0.0001f ? direction.normalized : Vector2.right;
        this.damage = Mathf.Max(1, damage);
        this.speed = Mathf.Max(0.01f, speed);
        this.lifetime = Mathf.Max(0.01f, lifetime);
        lifetimeTimer = this.lifetime;

        float angle = Mathf.Atan2(this.direction.y, this.direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void OnEnable()
    {
        lifetimeTimer = lifetime;
    }

    private void Update()
    {
        lifetimeTimer -= Time.deltaTime;
        if (lifetimeTimer <= 0f)
        {
            Destroy(gameObject);
            return;
        }

        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
        if (playerHealth == null) return;

        playerHealth.TakeDamage(damage);
        Destroy(gameObject);
    }
}
