using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float despawnDistanceSqr = 900f;

    private static Transform playerTransform;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        if (playerTransform == null)
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) playerTransform = playerObj.transform;
        }
    }

    void FixedUpdate()
    {
        if (playerTransform == null) return;

        Vector2 current = rb.position;
        Vector2 target = playerTransform.position;
        Vector2 delta = target - current;

        float distSqr = delta.x * delta.x + delta.y * delta.y;
        if (distSqr > despawnDistanceSqr)
        {
            EnemyPool.Instance.Release(gameObject);
            return;
        }

        float invLen = 1f / Mathf.Sqrt(distSqr);
        Vector2 dir = delta * invLen;
        rb.MovePosition(current + dir * moveSpeed * Time.fixedDeltaTime);
    }
}