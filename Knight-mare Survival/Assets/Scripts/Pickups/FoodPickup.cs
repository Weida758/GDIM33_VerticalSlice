using UnityEngine;

public class FoodPickup : MonoBehaviour
{
    [SerializeField] private int healAmount = 20;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
        if (playerHealth == null) return;

        playerHealth.Heal(healAmount);
        Destroy(gameObject);
    }
}
