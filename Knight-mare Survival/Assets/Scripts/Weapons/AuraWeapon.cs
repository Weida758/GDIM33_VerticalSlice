using UnityEngine;

public class AuraWeapon : Weapon
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float verticalOffset;
    [SerializeField] private float flashDuration = 0.1f;

    private readonly Collider2D[] hitBuffer = new Collider2D[32];
    private float cooldownTimer;
    private float flashTimer;

    private void Update()
    {
        if (Data == null) return;

        float deltaTime = Time.deltaTime;
        cooldownTimer -= deltaTime;
        flashTimer -= deltaTime;

        if (cooldownTimer <= 0f)
        {
            Attack();
            cooldownTimer = Stats.cooldown;
            flashTimer = flashDuration;
        }
    }

    private void Attack()
    {
        Vector2 attackCenter = GetAttackCenter();
        int hitCount = Physics2D.OverlapCircleNonAlloc(attackCenter, Stats.area, hitBuffer, enemyLayer);

        for (int i = 0; i < hitCount; i++)
        {
            EnemyHealth enemy = hitBuffer[i].GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(Stats.damage, attackCenter);
            }
        }
    }

    private Vector2 GetAttackCenter()
    {
        Vector2 basePosition = transform.position;
        return basePosition + new Vector2(0f, verticalOffset);
    }

    private void OnDrawGizmosSelected()
    {
        if (Data == null || Data.levels == null || Data.levels.Length == 0) return;

        if (flashTimer > 0f)
        {
            Gizmos.color = new Color(1f, 1f, 0.2f, 0.8f);
        }
        else
        {
            Gizmos.color = new Color(1f, 0.2f, 0.2f, 0.3f);
        }
        Gizmos.DrawWireSphere(GetAttackCenter(), Stats.area);
    }
}
