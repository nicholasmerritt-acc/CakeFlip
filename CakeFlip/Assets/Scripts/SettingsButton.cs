using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsButton : MonoBehaviour
{
    public GameObject settingsDisplay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        settingsDisplay.SetActive(false);
    }
    public void OnSettingsButtonClick()
    {
        if (settingsDisplay.activeInHierarchy)
        {
            settingsDisplay.SetActive(false);
        } 
        else
        {
            settingsDisplay.SetActive(true);
        }
        EventSystem.current.SetSelectedGameObject(settingsDisplay);
    }
}
