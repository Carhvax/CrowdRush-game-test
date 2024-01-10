using UnityEngine;

public class WeaponEffect : MonoBehaviour {
    [SerializeField] private ParticleSystem _shootEffect;

    public void ShootTarget(Vector3 target) {
        _shootEffect.Play();
    }
}
