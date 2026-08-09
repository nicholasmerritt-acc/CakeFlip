using UnityEngine;

public class MainMenuPlayButton : MonoBehaviour
{
    public void OnPlayButtonClick()
    {
        GameManager.Instance.TheAudioManager.PlayButtonPressClip();
        GameManager.Instance.LoadScene("Science");
    }
}
