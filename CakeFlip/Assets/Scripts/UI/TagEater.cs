using UnityEngine;

public class TagEater : MonoBehaviour
{
    [SerializeField] private string eatThisTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(eatThisTag))
        {
            Debug.Log("ate a " + eatThisTag);
            Destroy(other.gameObject);
        }
    }
}
