using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Lever : MonoBehaviour {
    private XRGrabInteractable _xrGrabInteractable;

    private Transform _parent;
    private Vector3 _lockedLocalScale;
    private Vector3 _lockedLocalPosition;

    private Transform grabbingHand = null;
    private bool isGrabbed = false;

    private void Awake() {
        _parent = transform.parent;
        _xrGrabInteractable = GetComponent<XRGrabInteractable>();
        
        _lockedLocalScale = transform.localScale;
        _lockedLocalPosition = transform.localPosition;
    }

    private void OnEnable() {
        _xrGrabInteractable.selectEntered.AddListener(OnGrab);
        _xrGrabInteractable.selectExited.AddListener(OnRelease);
    }
    private void OnDisable() {
        _xrGrabInteractable.selectEntered.RemoveListener(OnGrab);
        _xrGrabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void LateUpdate() {
        if (transform.parent != _parent)
            transform.SetParent(_parent, false);

        transform.localPosition = _lockedLocalPosition;
        transform.localScale = _lockedLocalScale;
        
        if (isGrabbed && grabbingHand != null)
            RotateLever();
    }

    private void OnGrab(SelectEnterEventArgs args) {
        grabbingHand = args.interactorObject.transform;
        isGrabbed = true;
    }
    private void OnRelease(SelectExitEventArgs args) {
        grabbingHand = null;
        isGrabbed = false;

        transform.localRotation = Quaternion.identity;
    }

    private void RotateLever()
    {
        // 1. Hand position in lever LOCAL space
        Vector3 localHandPos = _parent.InverseTransformPoint(grabbingHand.position);
        
        // 2. Project onto hinge plane (YZ plane, since X is hinge axis)
        localHandPos.x = 0f;

        // Avoid noise near pivot
        if (localHandPos.sqrMagnitude < 0.001f)
            return;

        // 3. Direction from pivot to hand
        Vector3 handDir = localHandPos.normalized;

        // 4. Cached / known rest direction (local +Y)
        Vector3 restDir = Vector3.up;

        // 5. Signed angle around local X axis
        float angle = Vector3.SignedAngle(
            restDir,
            handDir,
            Vector3.right   // local X axis (hinge)
        );
        angle = Mathf.Clamp(angle, -30f, 30f);

        // 6. Apply rotation (ONLY X axis)
        transform.localRotation = Quaternion.Euler(angle, 0f, 0f);
    }
    
    public float GetLeverValue() {
        // Get current lever angle around X
        float angle = transform.localEulerAngles.x;

        // Convert 0–360 to -180–180
        if (angle > 180f)
            angle -= 360f;

        // Map angle (-30 .. +30) → (-1 .. +1)
        return Mathf.InverseLerp(-30f, 30f, angle) * 2f - 1f;
    }
}
