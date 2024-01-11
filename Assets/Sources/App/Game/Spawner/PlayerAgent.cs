using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerAgent : MapAgent {
    
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Weapon _weapon;
    
    private float _aimTimer;

    public bool ShootTarget(Vector3 target, float weaponReload) {
        if ((_aimTimer -= Time.deltaTime) > 0) return false;

        ResetAim(weaponReload);
        
        _weapon.Shoot(target + Vector3.up);
        
        AttackAnimation();

        return true;
    }

    public void ResetAim(float weaponReload) {
        _aimTimer = weaponReload;
    }

    public override void Move(Vector3 velocity) {
        MoveAnimation(velocity.normalized);
        
        _agent.Move(velocity );
    }
    
    public void Rotate(Vector3 direction) {
        if(direction.magnitude != 0)
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }
}