using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    private void Start()
    {
        Slider slider = GetComponent<Slider>();
        slider.value = GameManager.Instance.TheAudioManager.MusicSource.volume;
    }

    public void SetMusicVolume(float amount)
    {
        GameManager.Instance.TheAudioManager.MusicSource.volume = amount;
    }
}
