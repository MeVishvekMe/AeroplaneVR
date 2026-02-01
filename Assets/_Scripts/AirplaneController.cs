using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AirplaneController : MonoBehaviour
{
    private Rigidbody rb;

    private float speed = 20f;
    private float pitchSpeed = 60f;
    private float maxPitchAngle = 45f;

    private void Awake() {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    private void FixedUpdate() {
        ApplyPitch();
        ApplyVelocity();
    }
    
    private void ApplyPitch() {
        float pitchInput = UserInput.Instance.physicalJoystickYInput;

        if (Mathf.Abs(pitchInput) < 0.001f)
            return;

        // Rotate around local X axis
        transform.Rotate(
            Vector3.right,
            pitchInput * pitchSpeed * Time.fixedDeltaTime,
            Space.Self
        );

        ClampPitch();
    }
    
    private void ClampPitch() {
        Vector3 euler = transform.localEulerAngles;

        float pitch = euler.x;
        if (pitch > 180f)
            pitch -= 360f;

        pitch = Mathf.Clamp(pitch, -maxPitchAngle, maxPitchAngle);

        transform.localEulerAngles = new Vector3(pitch, euler.y, euler.z);
    }
    
    private void ApplyVelocity() {
        rb.linearVelocity = transform.forward * speed;
    }
}