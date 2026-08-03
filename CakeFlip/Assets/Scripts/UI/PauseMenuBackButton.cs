using UnityEngine;

public class PauseMenuBackButton : MonoBehaviour
{
    public void OnBackClick()
    {
        GameManager.Instance.ThePauseGameHandler.TogglePause();
    }
}
