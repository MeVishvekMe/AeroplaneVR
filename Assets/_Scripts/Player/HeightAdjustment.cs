using System;
using UnityEngine;

public class HeightAdjustment : MonoBehaviour {
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Transform _playerVisual;
    [SerializeField] private Camera _camera;

    private void LateUpdate() {
        // Use LOCAL height (XR space)
        float cameraHeight = _camera.transform.localPosition.y;

        // CharacterController
        _characterController.height = cameraHeight;
        _characterController.center = new Vector3(0f, cameraHeight / 2f, 0f);
        
        // Head Visual
        _playerVisual.position = _camera.transform.position;
    }

}
