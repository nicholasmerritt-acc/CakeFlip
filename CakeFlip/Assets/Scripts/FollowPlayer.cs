using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    public GameObject player;

    private Vector3 offset = new Vector3(0f, 2f, -4f);
    private float zoomOutBoundary = -30f;
    private float zoomInBoundary = -4f;
    private float zoomIncrement = 1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ZoomIn();
        } 
        else if (Input.GetKeyDown(KeyCode.P))
        {
            ZoomOut();
        }

        transform.position = player.transform.position + offset;
    }

    //TODO the zoom is a lil wonky. change it to not go so far down when it goes out
    public void ZoomIn()
    {
        offset.z = Mathf.Min(zoomInBoundary, offset.z + zoomIncrement);
    }
    public void ZoomOut()
    {
        offset.z = Mathf.Max(zoomOutBoundary, offset.z - zoomIncrement);
    }
}
