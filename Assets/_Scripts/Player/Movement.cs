using UnityEngine;

public class Movement : MonoBehaviour {
    [Header("References")] 
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera headCamera;

    // Movement Settings
    private float _moveSpeed = 2.0f;

    // Gravity Settings
    private float _gravity = -9.81f;
    private float _groundStickForce = -2.0f;

    private float _verticalVelocity;

    private void Update() {
        // 1️⃣ Read joystick input (event-driven, stored in UserInput)
        Vector2 input = UserInput.Instance.leftJoystickInput;

        // 2️⃣ Head-relative movement direction
        Vector3 forward = headCamera.transform.forward;
        Vector3 right = headCamera.transform.right;

        // Ignore vertical look
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 horizontalMove =
            forward * input.y +
            right * input.x;

        // 3️⃣ Gravity handling
        if (characterController.isGrounded)
        {
            // Keep player grounded
            if (_verticalVelocity < 0f)
                _verticalVelocity = _groundStickForce;
        }
        else
        {
            // Apply gravity
            _verticalVelocity += _gravity * Time.deltaTime;
        }

        // Combine horizontal + vertical
        Vector3 move = horizontalMove * _moveSpeed;
        move.y = _verticalVelocity;

        // 4️⃣ Move CharacterController
        characterController.Move(move * Time.deltaTime);
    }
}