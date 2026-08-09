using UnityEngine;

public class MainMenuPlayButton : MonoBehaviour
{
    public void OnPlayButtonClick()
    {
        GameManager.Instance.TheAudioManager.PlayButtonPressClip();
        GameManager.Instance.RestoreProgress();
        string sceneToLoad = PlayerPrefs.GetString("CurrentScene", "Science");
        if (sceneToLoad == "MainMenu")
        {
            //this shouldn't happen. but, it does! so let's make sure we don't get stuck in an infinite main menu
            sceneToLoad = "Science";
        }
        GameManager.Instance.LoadScene(sceneToLoad);
    }
}
