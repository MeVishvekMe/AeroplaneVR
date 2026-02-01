using UnityEngine;

public class AirplaneController : MonoBehaviour {
    private Rigidbody rb;
    
    private float forwardForce = 25f;
    private float pitchTorque = 8f;   // up/down
    private float yawTorque = 6f;     // left/right
    private float rollTorque = 10f;   // banking
    private float rotationDamping = 2f;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        MoveForward();
        // RotateAirplane();
        // StabilizeRotation();
    }

    private void MoveForward() {
        rb.AddForce(transform.forward * forwardForce, ForceMode.Force);
    }

    private void RotateAirplane() {
        Vector2 input = UserInput.Instance.physicalJoystickInput;
        // input.x = roll / yaw
        // input.y = pitch

        Vector3 torque =
            transform.right   * (-input.y * pitchTorque) +
            transform.up      * ( input.x * yawTorque) +
            transform.forward * (-input.x * rollTorque);

        rb.AddTorque(torque, ForceMode.Force);
    }

    private void StabilizeRotation() {
        // Prevent infinite spinning / wobble
        rb.angularVelocity = Vector3.Lerp(
            rb.angularVelocity,
            Vector3.zero,
            rotationDamping * Time.fixedDeltaTime
        );
    }
}