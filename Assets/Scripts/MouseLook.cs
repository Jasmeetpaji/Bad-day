using UnityEngine;
using UnityEngine.InputSystem;
public class FirstPersonLook : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float mouseSensitivity = 0.15f;
    [Header("Camera Settings")]
    public float minPitch = -75f;
    public float maxPitch = 75f;
    private float pitch;
    private float yaw;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        if (Mouse.current == null)
            return;
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        yaw += mouseDelta.x * mouseSensitivity;
        pitch -= mouseDelta.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
