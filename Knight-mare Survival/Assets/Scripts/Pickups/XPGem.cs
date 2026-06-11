using UnityEngine;

public class XPGem : MonoBehaviour
{
    private static float globalMagnetEndTime;

    [SerializeField] private int xpValue = 1;
    [SerializeField] private float pickupRadius = 2f;
    [SerializeField] private float collectRadius = 0.8f;
    [SerializeField] private float magnetMaxSpeed = 12f;
    [SerializeField] private float magnetAcceleration = 30f;
    [SerializeField] private float globalMagnetMaxSpeed = 24f;
    [SerializeField] private float globalMagnetAcceleration = 80f;
    [SerializeField] private float settleDelay = 0.25f;

    private Transform playerTransform;
    private float currentMagnetSpeed;
    private float settleTimer;

    public static bool IsGlobalMagnetActive => Time.time < globalMagnetEndTime;

    public static void ActivateGlobalMagnet(float duration)
    {
        if (duration <= 0f) return;
        globalMagnetEndTime = Mathf.Max(globalMagnetEndTime, Time.time + duration);
    }

    private void OnEnable()
    {
        currentMagnetSpeed = 0f;
        settleTimer = settleDelay;
        playerTransform = ResolvePlayerTransform();
    }

    private Transform ResolvePlayerTransform()
    {
        if (PlayerXP.Instance != null)
        {
            return PlayerXP.Instance.PickupTarget;
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null) return null;
        return playerObject.transform;
    }

    private void Update()
    {
        if (playerTransform == null) return;

        Vector2 currentPosition = transform.position;
        Vector2 playerPosition = playerTransform.position;
        Vector2 toPlayer = playerPosition - currentPosition;
        float distanceSquared = toPlayer.sqrMagnitude;

        if (distanceSquared <= collectRadius * collectRadius)
        {
            Collect();
            return;
        }

        bool isGlobalMagnetActive = IsGlobalMagnetActive;

        if (settleTimer > 0f && !isGlobalMagnetActive)
        {
            settleTimer -= Time.deltaTime;
            return;
        }

        if (isGlobalMagnetActive || distanceSquared <= pickupRadius * pickupRadius)
        {
            float targetMaxSpeed = isGlobalMagnetActive ? globalMagnetMaxSpeed : magnetMaxSpeed;
            float acceleration = isGlobalMagnetActive ? globalMagnetAcceleration : magnetAcceleration;

            currentMagnetSpeed = Mathf.Min(targetMaxSpeed, currentMagnetSpeed + acceleration * Time.deltaTime);
            Vector2 directionToPlayer = toPlayer.normalized;
            transform.position = currentPosition + directionToPlayer * currentMagnetSpeed * Time.deltaTime;
        }
    }

    private void Collect()
    {
        if (PlayerXP.Instance != null)
        {
            PlayerXP.Instance.AddXP(xpValue);
        }

        if (XPGemPool.Instance != null)
        {
            XPGemPool.Instance.Release(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
