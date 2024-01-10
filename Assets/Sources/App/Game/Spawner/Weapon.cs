using UnityEngine;

public class Weapon : MonoBehaviour {
    [SerializeField] private WeaponEffect _originPoint;
    
    public void Shoot(Vector3 target) {
        _originPoint.ShootTarget(target);
    }
    
}
