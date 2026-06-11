using UnityEngine;

public class MagnetPickup : MonoBehaviour
{
    [SerializeField] private float magnetDuration = 4f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsPlayer(other)) return;

        XPGem.ActivateGlobalMagnet(magnetDuration);
        Destroy(gameObject);
    }

    private bool IsPlayer(Collider2D other)
    {
        return other.GetComponentInParent<PlayerHealth>() != null ||
               other.GetComponentInParent<PlayerXP>() != null ||
               other.CompareTag("Player");
    }
}
