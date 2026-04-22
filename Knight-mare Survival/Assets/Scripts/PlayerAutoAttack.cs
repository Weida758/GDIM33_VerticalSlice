using UnityEngine;

public class PlayerAutoAttack : MonoBehaviour
{
    public float attackInterval = 1f;
    public float attackRadius = 2f;
    public int damage = 1;
    public LayerMask enemyLayer;
    public float flashDuration = 0.1f;
    public float offset;

    private float timer;
    private float flashTimer;
    private Collider2D[] hitBuffer = new Collider2D[32];

    void Update()
    {
        float dt = Time.deltaTime;
        timer -= dt;
        flashTimer -= dt;

        if (timer <= 0f)
        {
            Attack();
            timer = attackInterval;
            flashTimer = flashDuration;
        }
    }

    void Attack()
    {
        int count = Physics2D.OverlapCircleNonAlloc(new Vector2(transform.position.x, transform.position.y + offset), attackRadius, hitBuffer, enemyLayer);
        if (count == 0) return;

        for (int i = 0; i < count; i++)
        {
            var enemy = hitBuffer[i].GetComponent<EnemyHealth>();
            if (enemy != null) enemy.TakeDamage(damage);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = flashTimer > 0f
            ? new Color(1f, 1f, 0.2f, 0.8f)
            : new Color(1f, 0.2f, 0.2f, 0.3f);
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y + offset), attackRadius);
    }
}