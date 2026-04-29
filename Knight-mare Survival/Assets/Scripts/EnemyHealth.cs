using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public float flashDuration = 0.1f;
    public float knockbackDuration = 0.15f;
    public float knockbackForce = 8f;

    private int currentHealth;
    private SpriteRenderer sr;
    private Color baseColor;
    private Rigidbody2D rb;
    private EnemyChase chase;

    private float flashTimer;
    private float knockbackTimer;

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        chase = GetComponent<EnemyChase>();
        if (sr != null) baseColor = sr.color;
    }

    void OnEnable()
    {
        currentHealth = maxHealth;
        flashTimer = 0f;
        knockbackTimer = 0f;
        if (sr != null) sr.color = baseColor;
        if (chase != null) chase.enabled = true;
    }

    void Update()
    {
        if (flashTimer > 0f)
        {
            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0f && sr != null) sr.color = baseColor;
        }

        if (knockbackTimer > 0f)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f && chase != null) chase.enabled = true;
        }
    }
    
    public void TakeDamage(int amount, Vector2 sourcePosition)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            EnemyPool.Instance.Release(gameObject);
            return;
        }

        if (sr != null)
        {
            sr.color = Color.red;
            flashTimer = flashDuration;
        }

        if (rb != null)
        {
            Vector2 dir = ((Vector2)transform.position - sourcePosition).normalized;
            if (chase != null) chase.enabled = false;
            rb.linearVelocity = dir * knockbackForce;
            knockbackTimer = knockbackDuration;
        }
    }
}