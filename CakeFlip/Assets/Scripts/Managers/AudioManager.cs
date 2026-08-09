using UnityEngine;
using Util;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    public AudioSource MusicSource;
    [SerializeField] private AudioClip backgroundMusic;

    [Header("SFX")]
    public AudioSource SfxSource;
    [SerializeField] private AudioClip buttonPressClip;
    [SerializeField] private AudioClip explosionClip;
    [SerializeField] private AudioClip[] audienceSFX;
    [SerializeField] private AudioClip[] skateboardRollClips;

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

    public void PlayAudienceClip()
    {
        //choose one of our random audience "ooh and ahh" clips
        if (audienceSFX.Length > 0)
        {
            SfxSource.PlayOneShot(audienceSFX.GetRandomItem());
        }
    }

    public void PlaySkateboardRollClip()
    {
        //choose one of our random skateboard rolling clips
        if (skateboardRollClips.Length > 0)
        {
            SfxSource.PlayOneShot(skateboardRollClips.GetRandomItem());
        }
    }

    private void PlayBackgroundMusic()
    {
        MusicSource.clip = backgroundMusic;
        MusicSource.loop = true;
        if (!MusicSource.isPlaying)
        {
            MusicSource.Play();
        }
    }

    public void PlayOneShot(AudioClip clip)
    {
        SfxSource.PlayOneShot(clip);
    }

}
