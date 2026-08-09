using UnityEngine;

public class TagEater : MonoBehaviour
{
    [SerializeField] private string eatThisTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(eatThisTag))
        {
            Destroy(other.gameObject);
        }
    }
}
