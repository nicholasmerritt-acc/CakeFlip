using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform target;

    [Header("Tweakable Values")]
    [SerializeField] private float yaw = 0f;
    [SerializeField] private float pitch = 15f;
    [SerializeField] private float maxPitch;
    [SerializeField] private float minPitch;
    [SerializeField] private float sensitivityX = 3f;
    [SerializeField] private float sensitivityY = 3f;
    [SerializeField] private float cameraOffsetDistance = 3f;
    [SerializeField] private Vector3 offset = new Vector3(0f, .5f, 0f);
    [SerializeField] private Vector3 skateboardOffset = new Vector3(0f, .5f, 0f);
    [SerializeField] private Vector3 guyOffset = new Vector3(0f, 2f, 0f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        PlayerControllerOpen.OnShapeshift += OnPlayerShapeshift;
    }

    private void OnDisable()
    {
        PlayerControllerOpen.OnShapeshift -= OnPlayerShapeshift;
    }

    private void OnPlayerShapeshift(bool skateboard)
    {
        if (skateboard)
        {
            offset = skateboardOffset;
        }
        else
        {
            offset = guyOffset;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetButtonDown("ZoomIn"))
        {
            ZoomIn();
        }
        else if (Input.GetButtonDown("ZoomOut"))
        {
            ZoomOut();
        }

        yaw += Input.GetAxisRaw("Mouse X") * sensitivityX;
        pitch -= Input.GetAxisRaw("Mouse Y") * sensitivityY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 focalPoint = target.transform.position + offset;

        // P - f*r*d?
        transform.position = focalPoint - targetRotation * Vector3.forward * cameraOffsetDistance;
        transform.rotation = targetRotation;
    }

    public void ZoomIn()
    {
        cameraOffsetDistance -= 1;
    }
    public void ZoomOut()
    {
        cameraOffsetDistance += 1;
    }
}
