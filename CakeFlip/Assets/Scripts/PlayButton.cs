using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void OnPlayButtonClick()
    {
        SceneManager.LoadScene("Level1");
    }
}
