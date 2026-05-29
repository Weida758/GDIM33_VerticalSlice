using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource soundFXSource;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void PlaySoundEffectClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        if (audioClip == null)
        {
            Debug.LogWarning("Tried to play a sound effect, but the AudioClip is missing.");
            return;
        }

        if (soundFXSource == null)
        {
            Debug.LogWarning("AudioManager has no soundFXSource assigned.");
            return;
        }

        soundFXSource.PlayOneShot(audioClip, volume);
    }
}