using UnityEngine;

public class CameraFacing : MonoBehaviour {
    
    private Transform _cameraTransform;
    private Camera _camera;

    private void Awake() {
        _camera = Camera.main;
        _cameraTransform = _camera.transform;
    }

    private void LateUpdate() {
        var cameraRotation = _cameraTransform.rotation;
        var transformPosition = transform.position;
        
        transform.LookAt(transformPosition + cameraRotation * Vector3.forward,  cameraRotation * Vector3.up);
        
        // TODO: Make offset relative to camera
    }

}
