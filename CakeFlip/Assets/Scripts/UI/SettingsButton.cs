using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;

    public void OnClick()
    {
        settingsPanel.SetActive(true);
    }
}
