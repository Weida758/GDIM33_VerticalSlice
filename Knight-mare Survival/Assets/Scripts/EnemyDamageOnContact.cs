using UnityEngine;

public class EnemyDamageOnContact : MonoBehaviour
{
    public int damage = 10;
    public float damageInterval = 0.5f;

    private float damageTimer;
    private PlayerHealth touchingPlayer;

    void OnEnable()
    {
        damageTimer = 0f;
        touchingPlayer = null;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var ph = collision.collider.GetComponent<PlayerHealth>();
        if (ph != null) touchingPlayer = ph;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        var ph = collision.collider.GetComponent<PlayerHealth>();
        if (ph != null && ph == touchingPlayer) touchingPlayer = null;
    }

    void Update()
    {
        if (damageTimer > 0f) damageTimer -= Time.deltaTime;

        if (touchingPlayer != null && damageTimer <= 0f)
        {
            touchingPlayer.TakeDamage(damage);
            damageTimer = damageInterval;
        }
    }
}