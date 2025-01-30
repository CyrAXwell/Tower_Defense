using UnityEngine;

public class AudioManager : MonoBehaviour
{  
    [Header("AudioSources")]
    [SerializeField] private AudioSource sFXSource;

    [Header("Sounds")]
    public AudioClip Shoot;

    public static AudioManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        sFXSource.PlayOneShot(clip, volume);
    }
}
