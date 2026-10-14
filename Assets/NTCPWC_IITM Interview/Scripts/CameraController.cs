using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float lookSpeed = 2f; // New variable for mouse sensitivity
    public enum CameraMode { Free, Follow }
    public CameraMode currentMode = CameraMode.Free;
    Camera mainCamera;
    public GameObject selectedObject;

    // Track rotation independent of the transform to prevent Z-axis tilt
    private float pitch = 0f;
    private float yaw = 0f;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        
        // Initialize pitch and yaw based on the camera's starting rotation
        Vector3 startAngles = transform.eulerAngles;
        pitch = startAngles.x;
        yaw = startAngles.y;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchCameraMode(CameraMode.Free);
        }
        if (currentMode == CameraMode.Free)
        {
            CameraMovement();
        }
    }

    public void CameraMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 1. Mouse Rotation Logic
        yaw += mouseX * lookSpeed;
        pitch -= mouseY * lookSpeed;
        pitch = Mathf.Clamp(pitch, -89f, 89f); // Prevents flipping upside down

        // Apply rotation directly
        mainCamera.transform.eulerAngles = new Vector3(pitch, yaw, 0f);

        // 2. Keyboard Movement Logic (Moves in the direction you are looking)
        Vector3 movement = (mainCamera.transform.right * horizontalInput) + (mainCamera.transform.forward * verticalInput);
        
        // Apply position
        mainCamera.transform.position += movement * moveSpeed * Time.deltaTime;
    }

    public void SwitchCameraMode(CameraMode newMode)
    {
        currentMode = newMode;
    }

    public void FollowSelectedObject(GameObject selectedObject)
    {
        if (selectedObject != null)
        {
            this.selectedObject = selectedObject;
            SwitchCameraMode(CameraMode.Follow);
        }
    }

    void LateUpdate()
    {
        if (currentMode == CameraMode.Follow && selectedObject != null)
        {
            Vector3 offset = (-selectedObject.transform.forward * 10f) + (Vector3.up * 5f);
            Vector3 newPosition = selectedObject.transform.position + offset; 
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, newPosition, Time.deltaTime * moveSpeed);
            mainCamera.transform.LookAt(selectedObject.transform);
        }
    }
}