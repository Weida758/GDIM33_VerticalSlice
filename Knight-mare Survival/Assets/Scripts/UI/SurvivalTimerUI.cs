using TMPro;
using UnityEngine;

public class SurvivalTimerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private string prefix;

    private float elapsedTime;
    private bool isRunning = true;

    private void Awake()
    {
        if (timerText == null)
        {
            timerText = GetComponent<TMP_Text>();
        }
    }

    private void OnEnable()
    {
        TryBindPlayerHealth();
        UpdateText();
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.onDied -= StopTimer;
        }
    }

    private void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;
        UpdateText();
    }

    private void TryBindPlayerHealth()
    {
        if (playerHealth == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                playerHealth = playerObject.GetComponent<PlayerHealth>();
            }
        }

        if (playerHealth != null)
        {
            playerHealth.onDied -= StopTimer;
            playerHealth.onDied += StopTimer;
        }
    }

    private void StopTimer()
    {
        isRunning = false;
        UpdateText();
    }

    private void UpdateText()
    {
        if (timerText == null) return;

        int totalSeconds = Mathf.FloorToInt(elapsedTime);
        int hours = totalSeconds / 3600;
        int minutes = totalSeconds / 60 % 60;
        int seconds = totalSeconds % 60;

        if (hours > 0)
        {
            timerText.text = $"{prefix}{hours:0}:{minutes:00}:{seconds:00}";
        }
        else
        {
            timerText.text = $"{prefix}{minutes:00}:{seconds:00}";
        }
    }
}
