using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public float invincibilityDuration = 0.5f;

    public UnityEvent<int, int> onHealthChanged;
    public event Action onDied;

    private float invincibilityTimer;

    public bool IsInvincible => invincibilityTimer > 0f;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        onHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Update()
    {
        if (invincibilityTimer > 0f) invincibilityTimer -= Time.deltaTime;
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || currentHealth <= 0 || IsInvincible) return;

        currentHealth = Mathf.Max(0, currentHealth - amount);
        invincibilityTimer = invincibilityDuration;
        onHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth == 0) onDied?.Invoke();
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || currentHealth <= 0) return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        onHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    [ContextMenu("Take 10 Damage")]
    void DebugTakeDamage() => TakeDamage(10);

    [ContextMenu("Kill")]
    void DebugKill() => TakeDamage(currentHealth);
}