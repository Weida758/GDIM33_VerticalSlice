using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUI : MonoBehaviour
{
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private PlayerHealth player;


    private void Start()
    {
        player.onDied += ShowDeathPanel;
    }

    private void OnDisable()
    {
        player.onDied -=  ShowDeathPanel;
    }
    
    
    private void ShowDeathPanel()
    {
        deathPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
