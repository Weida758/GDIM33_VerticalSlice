using UnityEngine;
using System;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    public static event Action OnPause;
    public static event Action OnResume;
    
    public void Pause()
    {
        
        pausePanel.SetActive(true);
        OnPause?.Invoke();
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        OnResume?.Invoke();
    }
}
