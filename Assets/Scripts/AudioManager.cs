using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip gameOverSFX;
    public AudioClip endGameSFX;
    public AudioClip buttonClickSFX;
    public AudioClip evolutionSFX;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Create AudioSources
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = 0.1f; // Adjust volume as needed

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.volume = 0.3f; // Adjust volume as needed
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        musicSource.Stop();
    }

    public void PlayGameOverSFX()
    {
        if (gameOverSFX != null)
        {
            sfxSource.PlayOneShot(gameOverSFX);
        }
    }

    public void PlayEndGameSFX()
    {
        if (endGameSFX != null)
        {
            sfxSource.PlayOneShot(endGameSFX);
        }
    }

    public void PlayButtonClickSFX()
    {
        if (buttonClickSFX != null)
        {
            sfxSource.PlayOneShot(buttonClickSFX);
        }
    }

    public void PlayEvolutionSFX()
    {
        if (evolutionSFX != null)
        {
            sfxSource.PlayOneShot(evolutionSFX);
        }
    }
}