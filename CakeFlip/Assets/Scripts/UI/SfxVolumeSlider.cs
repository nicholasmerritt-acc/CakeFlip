using UnityEngine;
using UnityEngine.UI;

public class SfxVolumeSlider : MonoBehaviour
{
    private void Start()
    {
        Slider slider = GetComponent<Slider>();
        //set the value so we remember the volume setting between settings pages e.g. main menu vs in game
        slider.value = GameManager.Instance.TheAudioManager.SfxSource.volume;
    }

    public void SetSFXVolume(float amount)
    {
        GameManager.Instance.TheAudioManager.SfxSource.volume = amount;
    }
}
