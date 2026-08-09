using UnityEngine;

public class PlaySkateBoardRollClipHere : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.TheAudioManager.PlaySkateboardRollClip();
        }
    }
}
