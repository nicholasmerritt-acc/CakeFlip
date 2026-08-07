using UnityEngine;

public class MainMenuPlayButton : MonoBehaviour
{
    public void OnPlayButtonClick()
    {
        GameManager.Instance.LoadScene("ScientistLab");
    }
}
