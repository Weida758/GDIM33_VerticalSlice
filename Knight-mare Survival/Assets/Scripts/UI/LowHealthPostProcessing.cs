using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LowHealthPostProcessing : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Volume postProcessingVolume;

    [Header("Health Settings")]
    [SerializeField, Range(0f, 1f)] private float lowHealthThreshold = 0.35f;

    [Header("Vignette Settings")]
    [SerializeField, Range(0f, 1f)] private float maxVignetteIntensity = 0.55f;
    [SerializeField, Range(0f, 1f)] private float vignetteSmoothness = 0.7f;
    [SerializeField] private Color lowHealthColor = Color.red;

    [Header("Animation")]
    [SerializeField] private float fadeSpeed = 6f;

    private Vignette vignette;
    private float targetIntensity;

    private void Awake()
    {
        if (postProcessingVolume == null)
        {
            Debug.LogError("LowHealthPostProcessing is missing a Volume reference.");
            return;
        }

        if (!postProcessingVolume.profile.TryGet(out vignette))
        {
            Debug.LogError("The assigned Volume Profile does not have a Vignette override.");
            return;
        }

        vignette.color.overrideState = true;
        vignette.intensity.overrideState = true;
        vignette.smoothness.overrideState = true;

        vignette.color.value = lowHealthColor;
        vignette.intensity.value = 0f;
        vignette.smoothness.value = vignetteSmoothness;
    }

    private void OnEnable()
    {
        if (playerHealth == null)
        {
            Debug.LogError("LowHealthPostProcessing is missing a PlayerHealth reference.");
            return;
        }

        playerHealth.onHealthChanged.AddListener(HandleHealthChanged);
        HandleHealthChanged(playerHealth.currentHealth, playerHealth.maxHealth);
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.onHealthChanged.RemoveListener(HandleHealthChanged);
        }
    }

    private void Update()
    {
        if (vignette == null) return;

        vignette.intensity.value = Mathf.Lerp(
            vignette.intensity.value,
            targetIntensity,
            Time.unscaledDeltaTime * fadeSpeed
        );
    }

    private void HandleHealthChanged(int currentHealth, int maxHealth)
    {
        if (maxHealth <= 0)
        {
            targetIntensity = maxVignetteIntensity;
            return;
        }

        float healthPercent = (float)currentHealth / maxHealth;

        if (healthPercent >= lowHealthThreshold)
        {
            targetIntensity = 0f;
            return;
        }

        float lowHealthAmount = 1f - healthPercent / lowHealthThreshold;
        targetIntensity = lowHealthAmount * maxVignetteIntensity;
    }
}