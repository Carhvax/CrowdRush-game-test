using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] private Transform _target;

    private void LateUpdate() {
        transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime * 20f);
    }
}
