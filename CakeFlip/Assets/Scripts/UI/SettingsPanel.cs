using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
    // do this in awake, not start, because we want this to happen but settings panel is disabled by default
    void Awake()
    {
        GameManager.Instance.ThePauseGameHandler.SettingsPanel = gameObject;
    }

    private void OnDestroy()
    {
        GameManager.Instance.ThePauseGameHandler.SettingsPanel = null;
    }
}
