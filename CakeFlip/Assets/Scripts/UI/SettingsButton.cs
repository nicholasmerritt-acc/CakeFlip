using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;

    public void OnClick()
    {
        GameManager.Instance.TheAudioManager.PlayButtonPressClip();
        settingsPanel.SetActive(true);
    }
}
