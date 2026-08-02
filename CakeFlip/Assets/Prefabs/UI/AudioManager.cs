using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    public AudioSource MusicSource;
    [SerializeField] private AudioClip backgroundMusic;

    [Header("SFX")]
    public AudioSource SfxSource;
    [SerializeField] private AudioClip buttonPressClip;
    [SerializeField] private AudioClip explosionClip;
    [SerializeField] private AudioClip skateboardRollClip;
    [SerializeField] private AudioClip skateboardJumpClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (backgroundMusic != null)
        {
            PlayBackgroundMusic();
        }
    }

    public void PlayButtonPressClip()
    {
        SfxSource.PlayOneShot(buttonPressClip);
    }

    private void PlayBackgroundMusic()
    {
        MusicSource.clip = backgroundMusic;
        MusicSource.loop = true;
        MusicSource.Play();
    }

    public void PlayOneShot(AudioClip clip)
    {
        SfxSource.PlayOneShot(clip);
    }

}
