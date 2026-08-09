using UnityEngine;

public class PauseMenuBackButton : MonoBehaviour
{
    public void OnResumeClick()
    {
        GameManager.Instance.TheAudioManager.PlayButtonPressClip();
        GameManager.Instance.ThePauseGameHandler.UnpauseGame();
    }
}
