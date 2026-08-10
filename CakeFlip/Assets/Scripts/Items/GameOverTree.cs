using UnityEngine;

public class GameOverTree : MonoBehaviour
{
    /// <summary>
    /// Whether this is the tree of life or death.
    /// </summary>
    public bool Life;

    //the illusion of choice. all roads lead back to the start, really. one just clears your progress, too.

    private void OnTriggerEnter(Collider other)
    {
        if (Life)
        {
            GameManager.Instance.StartNewGame();
        } 
        else
        {
            GameManager.Instance.LoadScene("Science");
        }
    }
}
