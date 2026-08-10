using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    public AudioSource MusicSource;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private AudioClip introVoiceover;
    [SerializeField] private float introDelay = 3f;

    [Header("SFX")]
    public AudioSource SfxSource;
    [SerializeField] private AudioClip buttonPressClip;
    [SerializeField] private AudioClip explosionClip;
    [SerializeField] private AudioClip[] audienceSFX;
    [SerializeField] private AudioClip[] skateboardRollClips;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManager.Instance.ThePauseGameHandler.IsIntroStarWarsScrollScene())
        {
            backgroundMusic = introVoiceover;
        }
        else
        {
            backgroundMusic = musicClip;
        }
        if (backgroundMusic != null)
        {
            PlayBackgroundMusic();
        }
    }

    public void PlayButtonPressClip()
    {
        SfxSource.PlayOneShot(buttonPressClip);
    }

    /// <summary>
    /// Choose one of our random audience "ooh and ahh" clips
    /// </summary>
    public void PlayAudienceClip()
    {
        if (audienceSFX.Length > 0)
        {
            SfxSource.PlayOneShot(audienceSFX.GetRandomItem());
        }
    }

    /// <summary>
    /// Choose one of our random skateboard rolling clips
    /// </summary>
    public void PlaySkateboardRollClip()
    {
        if (skateboardRollClips.Length > 0)
        {
            SfxSource.PlayOneShot(skateboardRollClips.GetRandomItem());
        }
    }

    /// <summary>
    /// Start the ever-present, much-too-loud background music, on loop.
    /// </summary>
    private void PlayBackgroundMusic()
    {
        MusicSource.clip = backgroundMusic;
        MusicSource.loop = true;
        if (!MusicSource.isPlaying)
        {
            MusicSource.Play();
        }
    }

    /// <summary>
    /// Play an audioclip once.
    /// </summary>
    public void PlayOneShot(AudioClip clip)
    {
        if (clip != null)
        {
            SfxSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// Play our famous voice actress' recounting of the events that took place leading up to this game's story...
    /// </summary>
    public void PlayIntroVoiceoverClip()
    {
        MusicSource.loop = false;
        MusicSource.Stop();
        MusicSource.clip = introVoiceover;
        MusicSource.PlayDelayed(introDelay);
    }

    public void StopMusic()
    {
        MusicSource.Stop();
    }

    private void OnEnable()
    {
        MusicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        SfxSource.volume = PlayerPrefs.GetFloat("SfxVolume", 1f);
        SceneManager.activeSceneChanged += ResetForNewScene;
    }

    /// <summary>
    /// Don't let SFX persist between scenes. We should, however, let background music persist.
    /// </summary>
    private void ResetForNewScene(Scene arg0, Scene arg1)
    {
        SfxSource.Stop();
    }

    private void OnDisable()
    {
        // Make sure we save the volume settings to playerprefs so we don't rupture our eardrums everytime. just the first time.
        PlayerPrefs.SetFloat("MusicVolume", MusicSource.volume);
        PlayerPrefs.SetFloat("SfxVolume", SfxSource.volume);
        SceneManager.activeSceneChanged -= ResetForNewScene;
    }
}
