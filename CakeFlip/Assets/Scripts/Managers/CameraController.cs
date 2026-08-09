using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target;
    private InputSystem_Actions inputActions;

    [Header("Position / Rotation")]
    [SerializeField] private float yaw = 0f;
    [SerializeField] private float pitch = 15f;
    [SerializeField] private float maxPitch;
    [SerializeField] private float minPitch;
    [SerializeField] private float oldInputSystemAdjustment = .05f;
    public bool LookAroundEnabled = true;

    [Header("Sensitivities")]
    [SerializeField] private float sensitivityX = 3f;
    [SerializeField] private float sensitivityY = 3f;

    [Header("Offsets")]
    [SerializeField] private float cameraOffsetDistance = 3f;
    [SerializeField] private float minZoom = 1f;
    [SerializeField] private float maxZoom = 7f;
    [SerializeField] private Vector3 offset = new Vector3(0f, .5f, 0f);

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (target == null)
        {
            target = FindAnyObjectByType<PlayerController>().transform;
        }
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Zoom.performed += ZoomPerformed;

        PlayerShapeshift.CameraOffsetChanged += CameraOffsetChanged;
        PauseGameHandler.GamePaused += ReleaseMouse;
        PauseGameHandler.GameUnpaused += CaptureMouse;
    }

    private void ReleaseMouse()
    {
        LookAroundEnabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CaptureMouse()
    {
        if (GameManager.Instance.ThePauseGameHandler.isMainMenu)
        {
            return;
        }
        LookAroundEnabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        PlayerShapeshift.CameraOffsetChanged -= CameraOffsetChanged;
        PauseGameHandler.GamePaused -= ReleaseMouse;
        PauseGameHandler.GameUnpaused -= CaptureMouse;

        inputActions.Player.Zoom.performed -= ZoomPerformed;
        inputActions.Player.Disable();
    }

    private void CameraOffsetChanged(Vector3 newOffset)
    {
        offset = newOffset;
    }

    private void ZoomPerformed(InputAction.CallbackContext context)
    {
        float zoomAmount = context.ReadValue<float>();
        cameraOffsetDistance += zoomAmount;
        cameraOffsetDistance = Mathf.Clamp(cameraOffsetDistance, minZoom, maxZoom);
    }

    // LateUpdate to prevent jittering. Update camera after player movement
    void LateUpdate()
    {
        if (LookAroundEnabled)
        {
            Vector2 mouseDelta = inputActions.Player.Look.ReadValue<Vector2>() * oldInputSystemAdjustment;

            float mouseX = mouseDelta.x;
            float mouseY = mouseDelta.y;

            yaw += mouseX * sensitivityX;
            pitch -= mouseY * sensitivityY;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 focalPoint = target.transform.position + offset;
            Vector3 targetPosition = focalPoint - targetRotation * Vector3.forward * cameraOffsetDistance;
            transform.SetPositionAndRotation(targetPosition, targetRotation);
        }

    }
}
