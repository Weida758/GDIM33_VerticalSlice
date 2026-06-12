using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [field: SerializeField] public float flashDuration { get; private set; } = 0.1f;

    [SerializeField] private EnemyStat enemyStat;
    [SerializeField] private AudioClip takeDamageSound;

    public event Action<Vector3> OnDied;
    private float currentHealth;
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
        currentHealth = enemyStat.maxHealth;
        flashTimer = 0f;
        knockbackTimer = 0f;
        if (sr != null) sr.color = baseColor;
        if (chase != null) chase.enabled = true;
    }

    public void ApplyHealthMultiplier(float multiplier)
    {
        multiplier = Mathf.Max(0.01f, multiplier);
        currentHealth = enemyStat.maxHealth * multiplier;
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
    
    public void TakeDamage(float amount, Vector2 sourcePosition)
    {
        currentHealth -= amount;
        AudioManager.instance.PlaySoundEffectClip(takeDamageSound,  transform, 0.6f);
        if (currentHealth <= 0)
        {
            OnDied?.Invoke(transform.position);
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
            rb.linearVelocity = dir * enemyStat.knockbackForce;
            knockbackTimer = enemyStat.knockbackDuration;
        }
    }
}
