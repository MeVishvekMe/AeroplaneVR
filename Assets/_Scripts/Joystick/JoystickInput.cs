using UnityEngine;

public class JoystickInput : MonoBehaviour {
    [SerializeField] private Transform handle; // joystick stick
    private float maxAngle = 20f; // must match joint limits

    private Quaternion restRotation;

    private void Awake() {
        restRotation = handle.localRotation;
    }
    
    public Vector2 GetInput() {
        // Rotation difference from rest
        Quaternion delta = Quaternion.Inverse(restRotation) * handle.localRotation;
        Vector3 euler = delta.eulerAngles;

        // Convert 0–360 to -180–180
        float x = NormalizeAngle(euler.x);
        float z = NormalizeAngle(euler.z);

        // Map to -1 to 1
        float forward = Mathf.Clamp(x / maxAngle, -1f, 1f);
        float right   = Mathf.Clamp(z / maxAngle, -1f, 1f);

        return new Vector2(right, forward);
    }

    private float NormalizeAngle(float angle) {
        if (angle > 180f)
            angle -= 360f;
        return angle;
    }
}
