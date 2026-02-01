using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour {
    public static UserInput Instance;

    private XRInput _xrInput;

    [SerializeField] private Lever _lever;
    
    public Vector2 leftJoystickInput { get; private set; }
    public float physicalJoystickYInput { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _xrInput = new XRInput();
        }
        else {
            Destroy(gameObject);
        }
    }

    private void OnEnable() {
        _xrInput.LeftController.Enable();
        _xrInput.LeftController.Joystick.performed += OnLeftJoystickValueChanged;
        _xrInput.LeftController.Joystick.canceled += OnLeftJoystickValueChanged;
    }
    private void OnDisable() {
        _xrInput.LeftController.Joystick.performed -= OnLeftJoystickValueChanged;
        _xrInput.LeftController.Joystick.canceled -= OnLeftJoystickValueChanged;
        _xrInput.LeftController.Disable();
    }

    private void Update() {
        physicalJoystickYInput = _lever.GetLeverValue();
    }

    private void OnLeftJoystickValueChanged(InputAction.CallbackContext ctx) {
        leftJoystickInput = ctx.ReadValue<Vector2>();
    }
}
